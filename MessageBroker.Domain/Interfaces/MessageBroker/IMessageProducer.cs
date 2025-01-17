using MessageBroker.Domain.Entities;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker.Domain.Interfaces.MessageBroker
{
    public interface IMessageProducer : IDisposable
    {
        Task PublishMessageAsync<T>(T message, string routingKey) where T : BaseMessage;
        void PublishMessage<T>(T message, string routingKey) where T : BaseMessage;
        Task PublishMessageWithCustomPropertise<T>(T message, string routingKey, IBasicProperties basicProperties) where T : BaseMessage;

    }
}
