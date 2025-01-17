using MessageBroker.Domain.Entities;
using MessageBroker.Domain.Interfaces;
using Microsoft.Extensions.Logging;
namespace MessageBroker.Application.Handler
{
    public class SMSMessageHandler : IMessageHandler<SMSMessage>
    {
        private readonly ILogger<SMSMessageHandler> _logger;
        private readonly ISMSService _smsService;

        public SMSMessageHandler(ILogger<SMSMessageHandler> logger, ISMSService smsService)
        {
            _logger = logger;
            _smsService = smsService;
        }

        public async Task HandleMessageAsync(SMSMessage message)
        {
            try
            {
                await _smsService.SendSMSAsync(message.PhoneNumber, message.Content);
                _logger.LogInformation($"SMS sent to {message.PhoneNumber}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending SMS");
                throw;
            }
        }
    }

}
