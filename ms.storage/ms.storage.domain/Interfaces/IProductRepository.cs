using ms.storage.domain.Entities;

namespace ms.storage.domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<bool> GetProductStock(string productName);
    }
}
