using Microsoft.Extensions.Configuration;
using ms.rabbitmq.Events;
using RabbitMQ.Client;
using System.Text;

namespace ms.rabbitmq.Producers
{
    public class EventProducer : IProducer
    {
        private readonly IConfiguration configuration;

        public EventProducer(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void SendMessage(IRabbitMqEvent rabbitmqEvent)
        {
            var factory = new ConnectionFactory()
            {
                HostName = configuration.GetValue<string>("Communication:EventBus:HostName")
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var queue = rabbitmqEvent.GetType().Name;
                channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false, null);
                var body = Encoding.UTF8.GetBytes(rabbitmqEvent.Serialize());
                channel.BasicPublish("", queue, null, body);
            }

        }
    }
}
