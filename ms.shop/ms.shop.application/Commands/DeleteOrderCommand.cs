using MediatR;
using ms.shop.application.DTOs;

namespace ms.shop.application.Commands
{
    public record DeleteOrderCommand(int id) : IRequest<bool>;
}
