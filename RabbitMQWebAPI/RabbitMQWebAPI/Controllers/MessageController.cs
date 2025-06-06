using Microsoft.AspNetCore.Mvc;
using RabbitMQWebAPI.Services;

namespace RabbitMQWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly RabbitMqProducer _producer;

        public MessageController(RabbitMqProducer producer)
        {
            _producer = producer;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromQuery] string routingKey = "info")
        {
            await _producer.SendMessageAsync(routingKey);
            return Ok($"Message sent with routing key: {routingKey}");
        }
    }
}
