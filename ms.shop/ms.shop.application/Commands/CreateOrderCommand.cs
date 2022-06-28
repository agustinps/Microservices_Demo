using MediatR;
using ms.shop.application.DTOs;

namespace ms.shop.application.Commands
{
    public record CreateOrderCommand(OrderCreateDto order) : IRequest<bool>;
}
