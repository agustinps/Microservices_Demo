using MediatR;

namespace ms.storage.application.Commands
{
    public record DeleteProductCommand(int id) : IRequest<bool>;
    
}
