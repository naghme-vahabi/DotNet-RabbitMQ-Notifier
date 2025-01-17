using MessageBroker.Domain.Enums;

namespace MessageBroker.Domain.Entities
{
    public abstract class BaseMessage
    {
        public MessageType Type { get; set; }
        public string? Content { get; set; }
        public required string To { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
