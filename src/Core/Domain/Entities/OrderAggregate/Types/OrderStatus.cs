using Ardalis.SmartEnum;
using eCommerceWeb.Domain.Exceptions;

namespace eCommerceWeb.Domain.Entities.OrderAggregate;

public sealed record class OrderStatus
{
    private OrderStatusEnum _status = null!;

    public OrderStatus(string value)
    {
        Value = Guard.Against.NullOrWhiteSpace(value, nameof(value));
    }

    private OrderStatus() { } // EF Core

    public string Value 
    {
        get => _status.Name;
        private init 
        { 
            Guard.Against.InvalidInput(
                value, 
                nameof(value), 
                x => OrderStatusEnum.TryFromName(x, true, out _status),
                ErrorMessages.Order.InvalidOrderStatus
            );
        }
    }

    public OrderStatus MoveTo(OrderStatus orderStatus)
    {
        if (_status.CanMoveToNext(orderStatus._status))
            throw new DomainException("Invalid order status change attempt from {0} to {1}", this, orderStatus);

        return orderStatus;
    }

    public static implicit operator string(OrderStatus orderStatus) => orderStatus.Value;

    public static OrderStatus Of(string value) => new(value);
    public static List<string> ListNames() => [.. OrderStatusEnum.List.Select(o => o.Name)];

    public static OrderStatus Pending() => new(OrderStatusEnum.Pending.Name);
    public static OrderStatus Fulfilled() => new(OrderStatusEnum.Fulfilled.Name);
    public static OrderStatus Cancelled() => new(OrderStatusEnum.Cancelled.Name);

    private abstract class OrderStatusEnum(string name, ushort value) 
        : SmartEnum<OrderStatusEnum, ushort>(name, value)
    {
        public static readonly OrderStatusEnum Pending = new PendingType();
        public static readonly OrderStatusEnum Fulfilled = new FulfilledType();
        public static readonly OrderStatusEnum Cancelled = new CancelledType();

        public bool CanMoveToNext(OrderStatusEnum next) 
            => AllowedTransitions.TryGetValue(this, out var allowed) && allowed.Contains(next);

        private static readonly Dictionary<OrderStatusEnum, HashSet<OrderStatusEnum>> AllowedTransitions = new()
        {
            [Pending] = [ Fulfilled, Cancelled ],
            [Fulfilled] = [ Pending ],
        };

        private sealed class PendingType() : OrderStatusEnum("Pending", 1);
        private sealed class FulfilledType() : OrderStatusEnum("Fulfilled", 2);
        private sealed class CancelledType() : OrderStatusEnum("Cancelled", 3);
    }
}