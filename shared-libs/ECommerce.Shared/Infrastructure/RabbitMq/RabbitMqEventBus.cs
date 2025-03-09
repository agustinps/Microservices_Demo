using System.Diagnostics;
using System.Text.Json;
using ECommerce.shared.Infrastructure.EventBus;
using ECommerce.shared.Infrastructure.EventBus.Abstraction;
using ECommerce.Shared.Infrastructure.RabbitMq;
using ECommerce.Shared.Observability;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;

namespace ECommerce.shared.Infrastructure.RabbitMq;
public class RabbitMqEventBus : IEventBus
{
    private const string ExchangeName = "ecommerce-exchange";
    private readonly IRabbitMqConnection _rabbitMqConnection;
    private readonly ActivitySource _activitySource;
    private readonly TextMapPropagator _propagator = Propagators.DefaultTextMapPropagator;
    public RabbitMqEventBus(IRabbitMqConnection rabbitMqConnection, RabbitMqTelemetry rabbitMqTelemetry)
    {
        _rabbitMqConnection = rabbitMqConnection;
        _activitySource = rabbitMqTelemetry.ActivitySource;
    }
    public Task PublishAsync(Event @event)
    {
        var routingKey = @event.GetType().Name;
        using var channel = _rabbitMqConnection.Connection.CreateModel();

        var activityName = $"{OpenTelemetryMessagingConventions.PublishOperation} {routingKey}";
        using var activity = _activitySource.StartActivity(activityName, ActivityKind.Client);
        ActivityContext activityContextToInject = default;
        if (activity is not null)
        {
            activityContextToInject = activity.Context;
        }
        var properties = channel.CreateBasicProperties();
        _propagator.Inject(new PropagationContext(activityContextToInject, Baggage.Current), properties,
            (properties, key, value) =>
            {
                properties.Headers ??= new Dictionary<string, object>();
                properties.Headers[key] = value;
            });
        SetActivityContext(activity, routingKey, OpenTelemetryMessagingConventions.PublishOperation);
        channel.ExchangeDeclare(
            exchange: ExchangeName,
            type: "fanout",
            durable: false,
            autoDelete: false,
            null);

        var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType());

        channel.BasicPublish(
            exchange: ExchangeName,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: properties,
            body: body);

        return Task.CompletedTask;
    }


    private static void SetActivityContext(Activity? activity, string routingKey, string operation)
    {
        if (activity is not null)
        {
            activity.SetTag(OpenTelemetryMessagingConventions.System, "rabbitmq");
            activity.SetTag(OpenTelemetryMessagingConventions.OperationName, operation);
            activity.SetTag(OpenTelemetryMessagingConventions.DestinationName, routingKey);
        }
    }
}
