using Ardalis.SmartEnum;

namespace eCommerceWeb.Domain.Entities.OrderAggregate;

public sealed record class OrderStatus
{
    private OrderStatusEnum _status = null!;
    public string Value 
    {
        get => _status.Name;
        private init { _status = Parse(value); }
    }

    public OrderStatus(string value)
    {
        Value = value;
    }

    private OrderStatus() { } // EF Core

    public bool MoveTo(string value)
    {
        var status = Parse(value);
        if (_status.CanMoveToNext(status))
        {
            _status = status;
            return true;
        }
        return false;
    }

    public static implicit operator string(OrderStatus orderStatus) => orderStatus.Value;
    public static IEnumerable<string> ListNames() => OrderStatusEnum.List.Select(o => o.Name);
    public static OrderStatus Of(string value) => new(value);

    public static OrderStatus Pending() => new(OrderStatusEnum.Pending.Name);
    public static OrderStatus Fulfilled() => new(OrderStatusEnum.Fulfilled.Name);
    public static OrderStatus Cancelled() => new(OrderStatusEnum.Cancelled.Name);

    private static OrderStatusEnum Parse(string? value)
    {
        Guard.Against.NullOrWhiteSpace(value, nameof(value));

        var success = OrderStatusEnum.TryFromName(value, true, out var parsed);
        Guard.Against.Expression(x => !x, success, ErrorMessages.Order.InvalidOrderStatus);

        return parsed;
    }

    private abstract class OrderStatusEnum(string name, ushort value) 
        : SmartEnum<OrderStatusEnum, ushort>(name, value)
    {
        public static readonly OrderStatusEnum Pending = new PendingType();
        public static readonly OrderStatusEnum Fulfilled = new FulfilledType();
        public static readonly OrderStatusEnum Cancelled = new CancelledType();

        public abstract bool CanMoveToNext(OrderStatusEnum next);

        private sealed class PendingType() : OrderStatusEnum("Pending", 1)
        {
            public override bool CanMoveToNext(OrderStatusEnum next) => 
                next == Fulfilled || next == Cancelled;
        }

        private sealed class FulfilledType() : OrderStatusEnum("Fulfilled", 2)
        {
            public override bool CanMoveToNext(OrderStatusEnum next) => next == Pending;
        }

        private sealed class CancelledType() : OrderStatusEnum("Cancelled", 3)
        {
            public override bool CanMoveToNext(OrderStatusEnum next) => false;
        }
    }
}
