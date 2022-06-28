using ms.rabbitmq.Events;
using System.Text.Json;

namespace ms.rabbitmq.Events
{
    public class OrderPreparedEvent : IRabbitMqEvent
    {
        public string Serialize() => JsonSerializer.Serialize(this);

        public Guid Id => Guid.NewGuid();
        public string Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public string Email { get; set; }

    }
}
