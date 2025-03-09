namespace ECommerce.Shared.Infrastructure.EventBus.Abstraction;
public class EventBusOptions
{
    public const string EventBusSectionName = "EventBus";
    public string QueueName { get; set; } = string.Empty;
}
