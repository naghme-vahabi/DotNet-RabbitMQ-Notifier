using MessageBroker.Domain.Entities;
using MessageBroker.Domain.Interfaces.MessageBroker;
using Microsoft.AspNetCore.Mvc;

namespace MessageBroker.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageProducer _messageProducer;

        public MessagesController(IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer;
        }

        [HttpPost("sms")]
        public async Task<IActionResult> SendSMS([FromBody] SMSMessage message)
        {
            await _messageProducer.PublishMessageAsync(message, "sms_route");
            return Ok();
        }

        [HttpPost("email")]
        public async Task<IActionResult> SendEmail([FromBody] EmailMessage message)
        {
            await _messageProducer.PublishMessageAsync(message, "email_route");
            return Ok();
        }
    }
}
