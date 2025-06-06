using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQWebAPI.Services
{
    public class RabbitMqConsumer : BackgroundService
    {
        private readonly ConnectionFactory _factory;

        public RabbitMqConsumer()
        {
            _factory = new ConnectionFactory
            {
                Uri = new Uri("amqps://mhfkboeu:NzXpu206MBff5csatdGTb10aUIVu_0z9@ostrich.lmq.cloudamqp.com/mhfkboeu")
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connection = await _factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            string exchangeName = "direct_exchange";
            await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Direct);

            string queueName = (await channel.QueueDeclareAsync()).QueueName;
            string routingKey = "info";
            await channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: routingKey);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[Consumer] Received '{message}' with routing key '{ea.RoutingKey}'");
                return Task.CompletedTask;
            };

            await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);

            Console.WriteLine("Consumer is waiting for messages...");
        }
    }
}
