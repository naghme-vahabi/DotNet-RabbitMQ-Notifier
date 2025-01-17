using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker.Domain.Entities
{
    public class SMSMessage : BaseMessage
    {
       public required string PhoneNumber { get; set; }
    }
}
