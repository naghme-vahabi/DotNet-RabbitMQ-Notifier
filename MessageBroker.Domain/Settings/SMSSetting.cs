using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker.Domain.Settings
{
    public class SMSSetting
    {
        public required string ApiEndpoint { get; set; }
        public required string ApiKey { get; set; }
        public required int Timeout { get; set; }
    }
}
