namespace ECommerce.Shared.Infrastructure.EventBus.Abstraction;
public class EventHandlerRegistration
{
    public Dictionary<string, Type> EventTypes { get; } = [];
}
