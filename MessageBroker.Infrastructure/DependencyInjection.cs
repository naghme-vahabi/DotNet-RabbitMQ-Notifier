using MessageBroker.Application.Handler;
using MessageBroker.Common.CommonServices;
using MessageBroker.Common.Enums;
using MessageBroker.Domain.Entities;
using MessageBroker.Domain.Interfaces;
using MessageBroker.Domain.Interfaces.MessageBroker;
using MessageBroker.Domain.Settings;
using MessageBroker.Infrastructure.MessageBroker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MessageBroker.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services)
        {
            services.AddHttpClient("SMSProvider", client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            // Services
            services.AddSingleton<IMessageProducer, MessageProducer>();
            services.AddScoped<IMessageHandler<SMSMessage>, SMSMessageHandler>();
            services.AddScoped<IMessageHandler<EmailMessage>, EmailMessageHandler>();

            services.AddSingleton<IMessageConsumer<SMSMessage>>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<MessageConsumer<SMSMessage>>>();
                var handler = sp.GetRequiredService<IMessageHandler<SMSMessage>>();
                var queueName = JsonReader.GetConfigs("appsetting", JsonType.Others, "RabbitMQSetting", "SmsQueueName");
                return new MessageConsumer<SMSMessage>(logger, queueName, handler);
            });

            services.AddSingleton<IMessageConsumer<EmailMessage>>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<MessageConsumer<EmailMessage>>>();
                var handler = sp.GetRequiredService<IMessageHandler<EmailMessage>>();
                var queueName = JsonReader.GetConfigs("appsetting", JsonType.Others, "RabbitMQSetting", "EmailQueueName");
                return new MessageConsumer<EmailMessage>(logger, queueName, handler);
            });

            services.AddHostedService<MessageConsumerHostedService>();

            return services;
        }
    }
}
