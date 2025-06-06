using RabbitMQ.Client;
using System.Text;

namespace RabbitMQWebAPI.Services
{
    public class RabbitMqProducer
    {
        private readonly ConnectionFactory _factory;
        private readonly string _exchangeName = "direct_exchange";

        public RabbitMqProducer()
        {
            _factory = new ConnectionFactory
            {
                Uri = new Uri("amqps://mhfkboeu:NzXpu206MBff5csatdGTb10aUIVu_0z9@ostrich.lmq.cloudamqp.com/mhfkboeu")
            };
        }

        public async Task SendMessageAsync(string routingKey)
        {
            var connection = await _factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(_exchangeName, ExchangeType.Direct);

            string message = "This is a direct exchange message : " + DateTime.Now.ToLongTimeString();
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: _exchangeName, routingKey: routingKey, body: body);

            Console.WriteLine($"Producer Sent '{message}' with routing key '{routingKey}'");
        }
    }
}
