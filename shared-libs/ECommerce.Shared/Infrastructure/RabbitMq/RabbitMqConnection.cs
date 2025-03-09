using RabbitMQ.Client;

namespace ECommerce.shared.Infrastructure.RabbitMq;
public class RabbitMqConnection : IDisposable, IRabbitMqConnection
{
    private readonly IConnection? _connection;
    private bool _disposed;

    public IConnection Connection => _connection!;

    public RabbitMqConnection(RabbitMqOptions options)
    {
        var factory = new ConnectionFactory
        {
            HostName = options.HostName
        };
        _connection = factory.CreateConnection();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _connection?.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
