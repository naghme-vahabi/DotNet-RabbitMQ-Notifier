namespace MessageBroker.Domain.Exceptions
{
    public class SMSSendException : Exception
    {
        public SMSSendException(string message) : base(message) { }
        public SMSSendException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
