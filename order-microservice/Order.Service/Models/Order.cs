using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Order.Service.Models;

public class Order
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid OrderId { get; private set; }
    [BsonElement("OrderProducts")]
    public List<OrderProduct> OrderProducts { get; private set; } = new();
    public required string CustomerId { get; init; }
    public DateTime OrderDate { get; private set; }
    public Order()
    {
        OrderId = Guid.NewGuid();
        OrderDate = DateTime.UtcNow;
    }


    public void AddOrderProduct(string productId, int quantity)
    {
        var existingOrderForProduct = OrderProducts.SingleOrDefault(o => o.ProductId == productId);
        if (existingOrderForProduct is not null)
            existingOrderForProduct.AddQuantity(quantity);
        else
        {
            var orderProduct = new OrderProduct { ProductId = productId };
            orderProduct.AddQuantity(quantity);
            OrderProducts.Add(orderProduct);
        }
    }


}
