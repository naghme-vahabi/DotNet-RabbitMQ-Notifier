using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker.Domain.Settings
{
    public class EmailSetting
    {
        public required string SmtpServer { get; set; }
        public required int Port { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string FromName { get; set; }
        public required string FromEmail { get; set; }
    }
}
