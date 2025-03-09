namespace Order.Service.Infrastructure.Data;

public interface IOrderStore
{
    public Task CreateOrderAsync(Models.Order order);
    Task<Models.Order?> GetCustomerOrderByIdAsync(string customerId, string orderId);
}
