using MediatR;
using ms.shop.application.DTOs;

namespace ms.shop.application.Queries
{
    public record GetAllOrdersQuery : IRequest<IEnumerable<OrderDto>>;
}
