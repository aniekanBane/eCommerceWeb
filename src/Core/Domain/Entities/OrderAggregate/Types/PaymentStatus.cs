using Ardalis.SmartEnum;
using eCommerceWeb.Domain.Exceptions;

namespace eCommerceWeb.Domain.Entities.OrderAggregate;

public sealed record class PaymentStatus
{
    private PaymentStatusEnum _status = null!;

    public PaymentStatus(string value)
    {
        Value = Guard.Against.NullOrWhiteSpace(value, nameof(value));
    }

    private PaymentStatus() { } // EF Core

    public string Value 
    {
        get => _status.Name;
        private init 
        {
            Guard.Against.InvalidInput(
                value, 
                nameof(value), 
                x => PaymentStatusEnum.TryFromName(value, true, out _status),
                ErrorMessages.Order.InvalidPaymentStatus
            );
        }
    }

    public PaymentStatus MoveTo(PaymentStatus paymentStatus)
    {
        if (_status.CanMoveToNext(paymentStatus._status))
            throw new DomainException("Invalid payment status change attempt from {0} to {1}", this, paymentStatus);

        return paymentStatus;
    }

    public static implicit operator string(PaymentStatus status) => status.Value;

    public static PaymentStatus Of(string value) => new(value);
    public static List<string> ListNames() => [.. PaymentStatusEnum.List.Select(x => x.Name)];

    public static PaymentStatus Paid() => new(PaymentStatusEnum.Paid.Name);
    public static PaymentStatus Failed() => new(PaymentStatusEnum.Failed.Name);
    public static PaymentStatus Pending() => new(PaymentStatusEnum.Pending.Name);
    public static PaymentStatus Refunded() => new(PaymentStatusEnum.Refunded.Name);
    public static PaymentStatus PartiallyRefunded() => new(PaymentStatusEnum.PartiallyRefunded.Name);

    private abstract class PaymentStatusEnum(string name, ushort value) 
        : SmartEnum<PaymentStatusEnum, ushort>(name, value)
    {
        public static readonly PaymentStatusEnum Pending = new PendingStatus();
        public static readonly PaymentStatusEnum Paid = new PaidStatus();
        public static readonly PaymentStatusEnum PartiallyRefunded = new PartiallyRefundedStatus();
        public static readonly PaymentStatusEnum Refunded = new RefundedStatus();
        public static readonly PaymentStatusEnum Failed = new FailedStatus();

        public bool CanMoveToNext(PaymentStatusEnum next) 
            => AllowedTransitions.TryGetValue(this, out var allowed) && allowed.Contains(next);

        private static readonly Dictionary<PaymentStatusEnum, HashSet<PaymentStatusEnum>> AllowedTransitions = new()
        {
            [Pending] = [ Paid, Failed ],
            [Paid] = [ PartiallyRefunded, Refunded ],
        };

        private sealed class PendingStatus() : PaymentStatusEnum("Pending", 10);
        private sealed class PaidStatus() : PaymentStatusEnum("Paid", 20);
        private sealed class PartiallyRefundedStatus() : PaymentStatusEnum("Partially Refunded", 30);
        private sealed class RefundedStatus() : PaymentStatusEnum("Refunded", 35);
        private sealed class FailedStatus() : PaymentStatusEnum("Failed", 40);
    }
}
