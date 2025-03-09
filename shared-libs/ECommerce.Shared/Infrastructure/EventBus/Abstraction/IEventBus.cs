namespace ECommerce.shared.Infrastructure.EventBus.Abstraction;
public interface IEventBus
{
    Task PublishAsync(Event @event);
}
