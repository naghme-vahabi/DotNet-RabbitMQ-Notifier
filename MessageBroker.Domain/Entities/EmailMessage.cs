using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker.Domain.Entities
{
    public class EmailMessage : BaseMessage
    {
        public required string Subject { get; set; }
        public required string From { get; set; }

    }
}
