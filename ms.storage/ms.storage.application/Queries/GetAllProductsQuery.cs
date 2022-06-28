using MediatR;
using ms.storage.application.DTOs;

namespace ms.storage.application.Queries
{
    public record GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>;
    
}
