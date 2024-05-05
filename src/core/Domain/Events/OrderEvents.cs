using eCommerceWeb.Domain.Entities.OrderAggregate;
using eCommerceWeb.Domain.Primitives.Events;
using eCommerceWeb.Domain.ValueObjects;

namespace eCommerceWeb.Domain.Events;

public sealed class OrderCreatedEvent(Order order) : CreatedEvent<Order>(order);

public sealed class OrderPaidEvent(Order order) : DomainEvent
{
    public Order Order { get; } = order;
}

public sealed class OrderCancelledEvent(Order order) : UpdatedEvent<Order>(order)
{
    public Order Order { get; } = order;
}

public sealed class OrderFulfilledEvent(Order order) : UpdatedEvent<Order>(order);

public sealed class PaymentRefundedEvent(Order order, Money amount) : UpdatedEvent<Order>(order)
{
    public Order Order { get; } = order;
    public Money Amount { get; } = amount;
}
