using Microsoft.EntityFrameworkCore;
using ms.storage.domain.Entities;
using ms.storage.domain.Interfaces;
using ms.storage.infrastructure.Data;

namespace ms.storage.infrastructure.Repository
{
    public class ProductRepository: Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext context;

        public ProductRepository(ApplicationDbContext context): base(context)
        {
            this.context = context;
        }

        public async Task<bool> GetProductStock(string productName)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.ProductName == productName);
            if (product is null)
                return false;
            return product.Stock > 0;
        }
    }
}
