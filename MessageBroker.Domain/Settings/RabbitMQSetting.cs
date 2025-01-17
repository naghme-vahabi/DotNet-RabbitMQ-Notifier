using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker.Domain.Settings
{
    public class RabbitMQSetting
    {
        public required string HostName { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string ExchangeName { get; set; }
        public required string SmsQueueName { get; set; }
        public required string EmailQueueName { get; set; }
    }
}
