using MediatR;
using ms.storage.application.DTOs;

namespace ms.storage.application.Queries
{
    public record GetProductByIdQuery(int id) : IRequest<ProductDto>;
    
}
