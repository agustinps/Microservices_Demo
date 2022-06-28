using AutoMapper;
using MediatR;
using ms.rabbitmq.Producers;
using ms.rabbitmq.Events;
using ms.shop.domain.Entities;
using ms.shop.domain.Interfaces;

namespace ms.shop.application.Commands.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        private readonly IProducer producer;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IProducer producer)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
            this.producer = producer;
        }

        public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            await orderRepository.AddAsync(mapper.Map<Order>(request.order));
            var response = await orderRepository.SaveChangesAsync();

            producer.SendMessage(mapper.Map<OrderCreatedEvent>(request.order));

            return response;
            
        }
        
    }
}
