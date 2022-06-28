using MediatR;
using ms.storage.application.DTOs;

namespace ms.storage.application.Commands
{
    public record CreateProductCommand(ProductCreateDto product) : IRequest<bool>;
    
}
