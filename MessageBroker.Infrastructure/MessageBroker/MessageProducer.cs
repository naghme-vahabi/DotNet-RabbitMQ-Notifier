using MessageBroker.Common.Enums;
using MessageBroker.Domain.Entities;
using MessageBroker.Domain.Interfaces.MessageBroker;
using MessageBroker.Domain.Settings;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using JsonReader = MessageBroker.Common.CommonServices.JsonReader;

namespace MessageBroker.Infrastructure.MessageBroker
{
    public class MessageProducer : IMessageProducer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _exchangeName;
        private readonly string _queueName;
        private RabbitMQSetting _rabbitMQSetting;
        private readonly ILogger<MessageProducer> _logger;

        private const string SMS_ROUTING_KEY = "sms_route";
        private const string EMAIL_ROUTING_KEY = "email_route";
        public MessageProducer(ILogger<MessageProducer> logger)
        {
            _logger = logger;
            try
            {
                _rabbitMQSetting = JsonReader.GetConfigs<RabbitMQSetting>("appsetting", JsonType.Entity);

                var factory = new ConnectionFactory()
                {
                    HostName = _rabbitMQSetting.HostName,
                    UserName = _rabbitMQSetting.UserName,
                    Password = _rabbitMQSetting.Password
                }; ;
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _exchangeName = _rabbitMQSetting.ExchangeName;

                // Declare Exchange
                _channel.ExchangeDeclare(
                     exchange: _exchangeName,
                     type: ExchangeType.Direct,
                     durable: true,
                     autoDelete: false,
                     arguments: null);

                // Declare Queues
                _channel.QueueDeclare(
                    queue: _rabbitMQSetting.SmsQueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false);

                _channel.QueueDeclare(
                    queue: _rabbitMQSetting.EmailQueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false);

                // Bind Queues
                _channel.QueueBind(_rabbitMQSetting.SmsQueueName, _exchangeName, SMS_ROUTING_KEY);
                _channel.QueueBind(_rabbitMQSetting.EmailQueueName, _exchangeName, EMAIL_ROUTING_KEY);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing RabbitMQ");
                throw;
            }

        }
        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }

        public void PublishMessage<T>(T message, string routingKey) where T : BaseMessage
        {
            try
            {
                var messageBody = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(messageBody);
                var properties = _channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.MessageId = Guid.NewGuid().ToString();
                properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                properties.ContentType = "application/json";

                if (string.IsNullOrEmpty(_exchangeName))
                {
                    _channel.BasicPublish(
                        exchange: "",
                        routingKey: routingKey,
                        basicProperties: properties,
                        body: body);
                }
                else
                {
                    _channel.BasicPublish(
                        exchange: _exchangeName,
                        routingKey: routingKey,
                        basicProperties: properties,
                        body: body);
                }
                _logger.LogInformation($"Message published successfully. Type: {message.Type}, RoutingKey: {routingKey}");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error publishing message");
                throw;
            }
        }

        public async Task PublishMessageAsync<T>(T message, string routingKey) where T : BaseMessage
        {
            Task.Run(()=> PublishMessage(message, routingKey));
        }

        public async Task PublishMessageWithCustomPropertise<T>(T message, string routingKey, global::RabbitMQ.Client.IBasicProperties basicProperties) where T : BaseMessage
        {
            try
            {
                var messageBody = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(messageBody);
                if (basicProperties is null)
                {
                   PublishMessage(message, routingKey);
                    return;
                }
                else
                {
                    if (string.IsNullOrEmpty(_exchangeName))
                    {
                        _channel.BasicPublish(
                            exchange: "",
                            routingKey: routingKey,
                            basicProperties: basicProperties,
                            body: body);
                    }
                    else
                    {
                        _channel.BasicPublish(
                            exchange: _exchangeName,
                            routingKey: routingKey,
                            basicProperties: basicProperties,
                            body: body);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Publishing a Message", ex);
            }
        }
    }
}
