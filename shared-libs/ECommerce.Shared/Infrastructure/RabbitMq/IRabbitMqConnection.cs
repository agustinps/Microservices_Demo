using RabbitMQ.Client;

namespace ECommerce.shared.Infrastructure.RabbitMq;
public interface IRabbitMqConnection
{
    IConnection Connection { get; }
}
