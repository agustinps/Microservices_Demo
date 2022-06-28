using AutoMapper;
using MediatR;
using ms.shop.application.DTOs;
using ms.shop.domain.Interfaces;

namespace ms.shop.application.Queries.Handlers
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly IMapper mapper;
        private readonly IOrderRepository orderRepository;

        public GetOrderByIdQueryHandler(IMapper mapper, IOrderRepository orderRepository)
        {
            this.mapper = mapper;
            this.orderRepository = orderRepository;
        }

        public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken) => mapper.Map<OrderDto>(await orderRepository.GetByIdAsync(request.id));
        
    }
}
