using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker.Domain.Exceptions
{
    public class EmailSendException : Exception
    {
        public EmailSendException(string message) : base(message) { }
        public EmailSendException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
