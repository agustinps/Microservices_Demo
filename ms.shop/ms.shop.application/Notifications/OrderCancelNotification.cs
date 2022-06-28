using MediatR;
using ms.shop.application.DTOs;

namespace ms.shop.application.Notifications
{
   public record OrderCancelNotification(OrderCreateDto order) : INotification;
    
}
