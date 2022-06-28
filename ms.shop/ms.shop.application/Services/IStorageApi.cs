using Refit;

namespace ms.shop.application.Services
{
    public interface IStorageApi
    {
        [Get("/api/Products/ProductStockAvaliable/{product}")]
        Task<bool> StockAvailable(string product);
    }
}
