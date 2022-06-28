using AutoMapper;
using MediatR;
using ms.rabbitmq.Consumers;
using ms.rabbitmq.Events;
using ms.shop.application.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ms.shop.api.Consumers
{
    public class OrderConsumer : IConsumer
    {
        private IConnection connection;
        private readonly IConfiguration configuration;
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IEmailSender emailSender;

        public OrderConsumer(IConfiguration configuration, IMediator mediator, IMapper mapper, IEmailSender emailSender)
        {
            this.configuration = configuration;
            this.mediator = mediator;
            this.mapper = mapper;
            this.emailSender = emailSender;
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

            SubscribeEvent(typeof(OrderPreparedEvent).Name, channel);
            SubscribeEvent(typeof(OrderShippedEvent).Name, channel);
        }

        private void SubscribeEvent(string queueName, IModel channel)
        {
            channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += ReceivedEvent;

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }


        private async void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            if (e.RoutingKey == typeof(OrderPreparedEvent).Name)
            {
                var message = Encoding.UTF8.GetString(e.Body.Span);
                var order = JsonSerializer.Deserialize<OrderPreparedEvent>(message);

                //Enviamos aviso por mail de pedido preparado
                await emailSender.SendOrderPreparedEmail(order.Email);                
            }
            if (e.RoutingKey == typeof(OrderShippedEvent).Name)
            {
                var message = Encoding.UTF8.GetString(e.Body.Span);
                var order = JsonSerializer.Deserialize<OrderShippedEvent>(message);

                //Enviamos aviso por mail de pedido enviado
                await emailSender.SendOrderShippedEMail(order.Email);
            }
        }


       

    }
}
