using Microsoft.EntityFrameworkCore;

namespace Product.Service.Infrastructure.Data.EntityFramework;

internal class ProductStore : IProductStore
{
    private readonly ProductContext _productContext;
    public ProductStore(ProductContext productContext)
    {
        _productContext = productContext;
    }
    public async Task<Models.Product?> GetById(int id)
        => await _productContext.Products
                                .Include(p => p.ProductType)
                                .FirstOrDefaultAsync(p => p.Id == id);

    public async Task CreateProduct(Models.Product product)
    {
        _productContext.Products.Add(product);
        await _productContext.SaveChangesAsync();
    }


    public async Task UpdateProduct(Models.Product product)
    {
        var existingProduct = await _productContext.FindAsync<Models.Product>(product.Id);
        if (existingProduct is not null)
        {
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;
            await _productContext.SaveChangesAsync();
        }
    }
}
