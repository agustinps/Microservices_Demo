using AutoMapper;
using MediatR;
using ms.storage.domain.Entities;
using ms.storage.domain.Interfaces;

namespace ms.storage.application.Commands.Handlers
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, bool>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public CreateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<bool> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            await productRepository.AddAsync(mapper.Map<Product>(request.product));
            return await productRepository.SaveChangesAsync();
        }
    }
}
