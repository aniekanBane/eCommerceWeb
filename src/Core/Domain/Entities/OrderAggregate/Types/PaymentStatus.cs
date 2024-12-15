using Ardalis.SmartEnum;

namespace eCommerceWeb.Domain.Entities.OrderAggregate;

public sealed record class PaymentStatus
{
    private PaymentStatusEnum _status = null!;
    public string Value 
    {
        get => _status.Name;
        private init { _status = Parse(value); }
    }

    public PaymentStatus(string value)
    {
        Value = value;
    }

    private PaymentStatus() { } // EF Core

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

    public static implicit operator string(PaymentStatus status) => status.Value;

    public static PaymentStatus Of(string value) => new(value);
    public static List<string> ListNames() => PaymentStatusEnum.List.Select(x => x.Name).ToList();

    public static PaymentStatus Paid() => new(PaymentStatusEnum.Paid.Name);
    public static PaymentStatus Failed() => new(PaymentStatusEnum.Failed.Name);
    public static PaymentStatus Pending() => new(PaymentStatusEnum.Pending.Name);
    public static PaymentStatus Refunded() => new(PaymentStatusEnum.Refunded.Name);

    private static PaymentStatusEnum Parse(string? value)
    {
        Guard.Against.NullOrWhiteSpace(value, nameof(value));

        var success = PaymentStatusEnum.TryFromName(value, true, out var parsed);
        Guard.Against.Expression(x => !x, success, ErrorMessages.Order.InvalidPaymentStatus);

        return parsed;
    }

    private abstract class PaymentStatusEnum(string name, ushort value) 
        : SmartEnum<PaymentStatusEnum, ushort>(name, value)
    {
        public static readonly PaymentStatusEnum Pending = new PendingType();
        public static readonly PaymentStatusEnum Paid = new PaidType();
        public static readonly PaymentStatusEnum Refunded = new RefundedType();
        public static readonly PaymentStatusEnum Failed = new FailedType();

        public abstract bool CanMoveToNext(PaymentStatusEnum next);

        private sealed class PendingType() : PaymentStatusEnum("Pending", 10)
        {
            public override bool CanMoveToNext(PaymentStatusEnum next) => 
                next == Paid || next == Failed;
        }

        private sealed class PaidType() : PaymentStatusEnum("Paid", 20)
        {
            public override bool CanMoveToNext(PaymentStatusEnum next) => next == Refunded;
        }

        private sealed class RefundedType() : PaymentStatusEnum("Refunded", 30)
        {
            public override bool CanMoveToNext(PaymentStatusEnum next) => false;
        }

        private sealed class FailedType() : PaymentStatusEnum("Failed", 40)
        {
            public override bool CanMoveToNext(PaymentStatusEnum next) => false;
        }
    }
}
