using MessageBroker.Domain.Entities;
using MessageBroker.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker.Infrastructure
{
    public class MessageConsumerHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public MessageConsumerHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var smsConsumer = _serviceProvider.GetRequiredService<IMessageConsumer<SMSMessage>>();
            var emailConsumer = _serviceProvider.GetRequiredService<IMessageConsumer<EmailMessage>>();

            // Start consumers
            await Task.WhenAll(
                Task.Run(() => smsConsumer.StartConsumingAsync(cancellationToken), cancellationToken),
                Task.Run(() => emailConsumer.StartConsumingAsync(cancellationToken), cancellationToken)
            );
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            var smsConsumer = _serviceProvider.GetRequiredService<IMessageConsumer<SMSMessage>>();
            var emailConsumer = _serviceProvider.GetRequiredService<IMessageConsumer<EmailMessage>>();

            await Task.WhenAll(
                smsConsumer.StopConsumingAsync(),
                emailConsumer.StopConsumingAsync()
            );
        }
    }
}
