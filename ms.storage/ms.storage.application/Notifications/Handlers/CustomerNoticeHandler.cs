using AutoMapper;
using MediatR;
using ms.rabbitmq.Events;
using ms.rabbitmq.Producers;

namespace ms.storage.application.Notifications.Handlers
{
    public class CustomerNoticeHandler : INotificationHandler<PreparedProductNotification>
    {
        private readonly IProducer producer;
        private readonly IMapper mapper;

        public CustomerNoticeHandler(IProducer producer, IMapper mapper)
        {
            this.producer = producer;
            this.mapper = mapper;
        }

        public Task Handle(PreparedProductNotification notification, CancellationToken cancellationToken)
        {
            //enviamos mensaje para avisar de pedido preparado
            producer.SendMessage(mapper.Map<OrderPreparedEvent>(notification.order));            
            return Task.CompletedTask;
        }
    }
}
