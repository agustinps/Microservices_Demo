using MediatR;
using ms.shop.application.Services;

namespace ms.shop.application.Notifications.Handler
{
    public class OrderCanceledEmailHandler : INotificationHandler<OrderCancelNotification>
    {
        private readonly IEmailSender emailSender;

        public OrderCanceledEmailHandler(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }


        public Task Handle(OrderCancelNotification notification, CancellationToken cancellationToken)
        {
            //Avisar cliente pedido cancelado
            emailSender.SendOrderCanceledEmail(notification.order.Email);
            return Task.CompletedTask;
        }

    }
}
