using AutoMapper;
using MediatR;
using ms.shop.application.DTOs;
using ms.shop.domain.Interfaces;

namespace ms.shop.application.Queries.Handlers
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<OrderDto>>
    {
        private readonly IMapper mapper;
        private readonly IOrderRepository orderRepository;

        public GetAllOrdersQueryHandler(IMapper mapper, IOrderRepository orderRepository)
        {
            this.mapper = mapper;
            this.orderRepository = orderRepository;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken) => mapper.Map<IEnumerable<OrderDto>>(await orderRepository.GetAllAsync());

    }
}
