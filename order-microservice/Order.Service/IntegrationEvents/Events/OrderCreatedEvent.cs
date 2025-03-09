using ECommerce.shared.Infrastructure.EventBus;

namespace Order.Service.IntegrationEvents.Events;

public record OrderCreatedEvent(string CustomerId) : Event;
