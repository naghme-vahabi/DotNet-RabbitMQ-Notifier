using MessageBroker.Domain.Interfaces;
using MessageBroker.Common.Enums;
using MessageBroker.Domain.Settings;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using JsonReader = MessageBroker.Common.CommonServices.JsonReader;
using MessageBroker.Domain.Entities;
using MessageBroker.Application.Handler;

namespace MessageBroker.Infrastructure.MessageBroker
{
    public class MessageConsumer<T> : IMessageConsumer<T> where T : BaseMessage
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<MessageConsumer<T>> _logger;
        private RabbitMQSetting _rabbitMQSetting;
        private readonly string _exchangeName;
        private readonly string _queueName;
        private readonly IMessageHandler<T> _messageHandler;

        public MessageConsumer(ILogger<MessageConsumer<T>> logger, string queueName, IMessageHandler<T> messageHandler)
        {
            _logger = logger;
            _queueName = queueName;
            _messageHandler = messageHandler;

            try
            {
                _rabbitMQSetting = JsonReader.GetConfigs<RabbitMQSetting>("appsetting", JsonType.Entity);

                var factory = new ConnectionFactory()
                {
                    HostName = _rabbitMQSetting.HostName,
                    UserName = _rabbitMQSetting.UserName,
                    Password = _rabbitMQSetting.Password
                };
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _exchangeName = _rabbitMQSetting.ExchangeName;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing RabbitMQ consumer");
                throw;
            }
        }

        public async Task StartConsumingAsync(CancellationToken cancellationToken)
        {
            var _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(body));
                    await _messageHandler.HandleMessageAsync(message);
                    _channel.BasicAck(ea.DeliveryTag, false);
                    _logger.LogInformation($"Message processed successfully. Type: {message.Type}");
                }

                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message");
                    _channel.BasicNack(ea.DeliveryTag, false, requeue: true);
                }
            };
            if (!string.IsNullOrEmpty(_queueName))
            {
                _channel.BasicConsume(queue: _queueName,
                                     autoAck: false,
                                     consumer: _consumer);
                await Task.Delay(-1, cancellationToken);
            }

        }

        public Task StopConsumingAsync()
        {
            _channel?.Close();
            _connection?.Close();
            return Task.CompletedTask;
        }

    }
}
