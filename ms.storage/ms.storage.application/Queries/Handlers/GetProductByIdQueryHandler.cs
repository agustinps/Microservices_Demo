using AutoMapper;
using MediatR;
using ms.storage.application.DTOs;
using ms.storage.domain.Interfaces;

namespace ms.storage.application.Queries.Handlers
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken) => mapper.Map<ProductDto>(await productRepository.GetByIdAsync(request.id));
        
    }
}
