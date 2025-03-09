using ECommerce.shared.Infrastructure.EventBus;

namespace ECommerce.Shared.Infrastructure.EventBus.Abstraction;
public interface IEventHandler<in TEvent> : IEventHandler
    where TEvent : Event
{
    Task Handle(TEvent @event);
    Task IEventHandler.Handle(Event @event) => Handle((TEvent)@event);
}
public interface IEventHandler
{
    Task Handle(Event @event);
}
