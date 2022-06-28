using MediatR;
using ms.storage.domain.Interfaces;

namespace ms.storage.application.Commands.Handlers
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            await productRepository.DeleteAsync(request.id);
            return await productRepository.SaveChangesAsync();
        }
    }
}
