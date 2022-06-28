using AutoMapper;
using MediatR;
using ms.rabbitmq.Consumers;
using ms.rabbitmq.Events;
using ms.storage.application.Notifications;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ms.storage.api.Consumers
{
    public class ProductConsumer : IConsumer
    {
        private IConnection connection;
        private readonly IConfiguration configuration;
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public ProductConsumer(IConfiguration configuration, IMediator mediator, IMapper mapper)
        {
            this.configuration = configuration;
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public void Unsubscribe() => connection?.Dispose();


        public void Subscribe()
        {            
            var factory = new ConnectionFactory()
            {
                HostName = configuration.GetValue<string>("Communication:EventBus:HostName")
            };
            connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            //var queue = typeof(OrderCreatedEvent).Name;
            //channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false, null);            

            //var consumer = new EventingBasicConsumer(channel);
            //consumer.Received += ReceivedEvent;

            //channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);
            
            SubscribeEvent(typeof(OrderCreatedEvent).Name, channel);                        
        }

        private async void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            if (e.RoutingKey == typeof(OrderCreatedEvent).Name)
            {
                var message = Encoding.UTF8.GetString(e.Body.Span);
                var order = JsonSerializer.Deserialize<OrderCreatedEvent>(message);

                await mediator.Publish(new PreparedProductNotification(order)); 
            }            
        }


        private void SubscribeEvent(string queueName, IModel channel)
        {            
            channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += ReceivedEvent;

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }



    }
}
