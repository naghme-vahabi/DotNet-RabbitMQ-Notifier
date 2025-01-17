using MessageBroker.Domain.Entities;
namespace MessageBroker.Application.Handler
{
    public interface IMessageHandler<in T> where T : BaseMessage
    {
        Task HandleMessageAsync(T message);
    }

}
