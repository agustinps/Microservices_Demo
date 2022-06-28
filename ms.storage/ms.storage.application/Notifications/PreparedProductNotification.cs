using MediatR;
using ms.rabbitmq.Events;

namespace ms.storage.application.Notifications
{
    public record PreparedProductNotification(OrderCreatedEvent order) : INotification;
}
