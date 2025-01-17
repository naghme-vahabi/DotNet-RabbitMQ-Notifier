using MessageBroker.Common.CommonServices;
using MessageBroker.Common.Enums;
using MessageBroker.Domain.Exceptions;
using MessageBroker.Domain.Interfaces;
using MessageBroker.Domain.Settings;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace MessageBroker.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly EmailSetting _emailSettings;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
            _emailSettings = JsonReader.GetConfigs<EmailSetting>("appsetting", JsonType.Entity);
        }

        /// <summary>
        ///  Send Email
        /// </summary>
        /// <param name="to">Reciever</param>
        /// <param name="subject">Subject</param>
        /// <param name="content">Email Content</param>
        /// <returns></returns>
        /// <exception cref="EmailSendException">Failed to send email</exception>
        public async Task SendEmailAsync(string to, string subject, string content)
        {
            try
            {
                using var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password)
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
                    Subject = subject,
                    Body = content,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(to);

                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation($"Email sent successfully to {to}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send email to {to}. Error: {ex.Message}");
                throw new EmailSendException($"Failed to send email to {to}", ex);
            }
        }
        /// <summary>
        /// Send Email With an Attachment
        /// </summary>
        /// <param name="to">Reciever</param>
        /// <param name="subject">Subject</param>
        /// <param name="content">Email Content</param>
        /// <param name="attachmentPath">Attachment Path</param>
        /// <returns></returns>
        /// <exception cref="EmailSendException">Failed to send email</exception>
        public async Task SendEmailWithAttachmentAsync(string to, string subject, string content, string attachmentPath)
        {
            try
            {
                using var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password)
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
                    Subject = subject,
                    Body = content,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(to);

                if (File.Exists(attachmentPath))
                {
                    mailMessage.Attachments.Add(new Attachment(attachmentPath));
                }

                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation($"Email with attachment sent successfully to {to}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send email with attachment to {to}. Error: {ex.Message}");
                throw new EmailSendException($"Failed to send email with attachment to {to}", ex);
            }
        }

    }
}
