using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Order.Service.Infrastructure.Data.MongoDb;

public class MongoOrderStore : IOrderStore
{
    private readonly IMongoCollection<Models.Order> _ordersCollection;

    public MongoOrderStore(IOptions<MongoDbSettings> mongoDbSettings)
    {
        var settings = MongoClientSettings.FromConnectionString(mongoDbSettings.Value.ConnectionString);
        settings.RetryWrites = true;
        settings.RetryReads = true;
        settings.MaxConnectionIdleTime = TimeSpan.FromMinutes(2);

        var mongoClient = new MongoClient(settings);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _ordersCollection = mongoDatabase.GetCollection<Models.Order>("Orders");
    }


    public async Task CreateOrderAsync(Models.Order order)
    {
        // _ordersCollection.InsertOne(order);
        await ExecuteWithRetryAsync(() => _ordersCollection.InsertOneAsync(order));
    }


    public async Task<Models.Order?> GetCustomerOrderByIdAsync(string customerId, string orderId)
    {
        var filter = Builders<Models.Order>.Filter.And(
            Builders<Models.Order>.Filter.Eq(o => o.CustomerId, customerId),
            Builders<Models.Order>.Filter.Eq(o => o.OrderId, Guid.Parse(orderId))
        );
        //return _ordersCollection.Find(filter).FirstOrDefault();
        return await ExecuteWithRetryAsync(() => _ordersCollection.Find(filter).FirstOrDefaultAsync());
    }


    private static async Task<bool> ExecuteWithRetryAsync(Func<Task> operation, int maxRetries = 3, int delayMs = 2000)
    {
        int retryCount = 0;
        bool success = false;

        while (retryCount < maxRetries && !success)
        {
            try
            {
                await operation();
                success = true;
            }
            catch (Exception ex)
            {
                retryCount++;
                Console.WriteLine($"Intento {retryCount} fallido: {ex.Message}");

                if (retryCount >= maxRetries) //Número máximo de reintentos alcanzado.
                    break;

                await Task.Delay(delayMs);
            }
        }

        return success;
    }



    private static async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> operation, int maxRetries = 3, int delayMs = 2000)
    {
        int retryCount = 0;
        T result = default!;

        while (retryCount < maxRetries)
        {
            try
            {
                result = await operation();
                break;
            }
            catch (Exception ex)
            {
                retryCount++;
                Console.WriteLine($"Intento {retryCount} fallido: {ex.Message}");

                if (retryCount >= maxRetries) //Número máximo de reintentos alcanzado.
                    break;

                await Task.Delay(delayMs);
            }
        }

        return result!;
    }



}
