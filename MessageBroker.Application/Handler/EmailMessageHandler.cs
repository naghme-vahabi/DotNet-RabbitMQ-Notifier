using MessageBroker.Domain.Entities;
using MessageBroker.Domain.Interfaces;
using Microsoft.Extensions.Logging;
namespace MessageBroker.Application.Handler
{
    public class EmailMessageHandler : IMessageHandler<EmailMessage>
    {
        private readonly ILogger<EmailMessageHandler> _logger;
        private readonly IEmailService _emailService;

        public EmailMessageHandler(ILogger<EmailMessageHandler> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public async Task HandleMessageAsync(EmailMessage message)
        {
            try
            {
                await _emailService.SendEmailAsync(message.To, message.Subject, message.Content);
                _logger.LogInformation($"Email sent to {message.To}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email");
                throw;
            }
        }
    }

}
