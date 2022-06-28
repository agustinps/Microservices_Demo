using Refit;

namespace ms.shop.application.Services
{
    public class StorageService : IStorageApi
    {
        private readonly IStorageApi storageApi;
        public StorageService()
        {                        
            storageApi = RestService.For<IStorageApi>("https://localhost:7236");              
        }

        public async Task<bool> StockAvailable(string product) => await storageApi.StockAvailable(product);
        
    }
}
