using MessageBroker.Common.CommonServices;
using MessageBroker.Common.Enums;
using MessageBroker.Domain.Exceptions;
using MessageBroker.Domain.Interfaces;
using MessageBroker.Domain.Settings;
using Microsoft.Extensions.Logging;

namespace MessageBroker.Infrastructure.Services
{
    public class SMSService : ISMSService
    {
        private readonly ILogger<SMSService> _logger;
        private readonly SMSSetting _smsSettings;
        private readonly HttpClient _httpClient;

        public SMSService(ILogger<SMSService> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _smsSettings = JsonReader.GetConfigs<SMSSetting>("appsetting", JsonType.Entity);
            _httpClient = httpClientFactory.CreateClient("SMSProvider");
        }
        /// <summary>
        /// Send SMS
        /// </summary>
        /// <param name="phoneNumber">Phone Number</param>
        /// <param name="content">Sms Content</param>
        /// <returns></returns>
        /// <exception cref="SMSSendException"></exception>
        public async Task SendSMSAsync(string phoneNumber, string content)
        {
            try
            {
                //TODO : Validate PhoneNumebr
                var request = new HttpRequestMessage(HttpMethod.Post, _smsSettings.ApiEndpoint)
                {
                    Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "to", phoneNumber },
                    { "message", content },
                    { "api_key", _smsSettings.ApiKey }
                })
                };

                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    throw new SMSSendException($"SMS provider returned status code: {response.StatusCode}");
                }
                _logger.LogInformation($"SMS sent successfully to {phoneNumber}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send SMS to {phoneNumber}. Error: {ex.Message}");
                throw new SMSSendException($"Failed to send SMS to {phoneNumber}", ex);
            }
        }

    }
}
