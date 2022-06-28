using AutoMapper;
using MediatR;
using ms.storage.application.DTOs;
using ms.storage.domain.Interfaces;

namespace ms.storage.application.Queries.Handlers
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public GetAllProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken) => mapper.Map<IEnumerable<ProductDto>>(await productRepository.GetAllAsync());
        
    }
}
