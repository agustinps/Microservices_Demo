using MediatR;
using ms.shop.application.DTOs;

namespace ms.shop.application.Queries
{
    public record GetOrderByIdQuery(int id) : IRequest<OrderDto>;    
}
