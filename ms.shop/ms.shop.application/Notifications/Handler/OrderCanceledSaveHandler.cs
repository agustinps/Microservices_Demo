using AutoMapper;
using MediatR;
using ms.shop.domain.Entities;
using ms.shop.domain.Interfaces;

namespace ms.shop.application.Notifications.Handler
{
    public class OrderCanceledSaveHandler : INotificationHandler<OrderCancelNotification>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;

        public OrderCanceledSaveHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
        }


        public async Task Handle(OrderCancelNotification notification, CancellationToken cancellationToken)
        {
            var orderAux = notification.order;
            //guardamos el pedido pero con cantidad y precio 0
            //en email el texto "pedido cancelado por falta de stock" para que quede registrado, realmente debería ser en campo notas o incidencias, pero esto es una demo.....
            orderAux.Price = 0;
            orderAux.Quantity = 0;
            orderAux.Amount = 0;
            orderAux.Email = "Pedido cancelado por falta de stock";
            await orderRepository.AddAsync(mapper.Map<Order>(orderAux));
        }
    }
}
