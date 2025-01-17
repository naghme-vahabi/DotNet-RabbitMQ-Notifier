using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker.Domain.Interfaces
{
    public interface ISMSService
    {
        Task SendSMSAsync(string phoneNumber, string content);
    }
}
