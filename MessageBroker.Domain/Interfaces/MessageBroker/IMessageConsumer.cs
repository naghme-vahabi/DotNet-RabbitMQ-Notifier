using MessageBroker.Domain.Entities;

namespace MessageBroker.Domain.Interfaces
{
    public interface IMessageConsumer<T> where T : BaseMessage
    { 
        Task StartConsumingAsync(CancellationToken cancellationToken);
        Task StopConsumingAsync();
    }
}
