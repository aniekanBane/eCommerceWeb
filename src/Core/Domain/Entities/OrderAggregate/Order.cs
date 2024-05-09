using eCommerceWeb.Domain.Events;
using eCommerceWeb.Domain.Exceptions;
using eCommerceWeb.Domain.Primitives.Entities;
using eCommerceWeb.Domain.ValueObjects;

namespace eCommerceWeb.Domain.Entities.OrderAggregate;

public sealed class Order : AuditableEntityWithDomainEvent<Guid>, IAggregateRoot
{   
    #pragma warning disable CS8618
    private Order() { } // EF Core

    public Order(Guid customerId, Address billingAddress, Address deliveryAddress) 
    {
        Guard.Against.NullOrDefault(customerId, nameof(customerId));
        
        Id = Guid.NewGuid();
        CustomerId = customerId;
        BillingAddress = billingAddress;
        DeliveryAddress = deliveryAddress;
        OrderStatus = OrderStatus.Pending();
        PaymentStatus = PaymentStatus.Pending();
    }

    public Guid CustomerId { get; private set; }
    public int OrderNo { get; private set; }
    public string? Note { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; }
    public Address BillingAddress { get; private set; }
    public Address DeliveryAddress { get; private set; }

    private readonly List<OrderItem> _orderItems = [];
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public Order AddItems(IEnumerable<OrderItem> orderItems) 
    { 
        _orderItems.AddRange(orderItems);
        return this;
    }

    public decimal GetTotal() => 
        _orderItems.Sum(x => x.Quantity * x.UnitPrice.Amount);

    public void SetFulfilledStatus()
    {
        if (!OrderStatus.MoveTo(OrderStatus.Fulfilled()))
        {
            OrderStatusException(OrderStatus.Fulfilled());
        }

        Note = "Order shipped.";
        RaiseDomainEvent(new OrderFulfilledEvent(this));
    }

    public void SetCancelledStatus()
    {
        if (!OrderStatus.MoveTo(OrderStatus.Cancelled()))
        {
            OrderStatusException(OrderStatus.Cancelled());
        }

        Note = "Order cancelled.";
        RaiseDomainEvent(new OrderCancelledEvent(this));
    }

    public void SetFailedStatus()
    {
        if (!PaymentStatus.MoveTo(PaymentStatus.Failed()))
        {
            PaymentStatusException(PaymentStatus.Failed());
        }

        SetCancelledStatus();
    }

    public void SetPaidStatus()
    {
        Guard.Against.InvalidInput(OrderStatus, nameof(OrderStatus), os => os == OrderStatus.Pending());
        if (!PaymentStatus.MoveTo(PaymentStatus.Paid()))
        {
            PaymentStatusException(PaymentStatus.Paid());
        }

        Note = "Order paid.";
        RaiseDomainEvent(new OrderPaidEvent(this));
    }

    public void SetRefundedStatus()
    {
        Guard.Against.InvalidInput(OrderStatus, nameof(OrderStatus), os => os != OrderStatus.Cancelled());
        if (!PaymentStatus.MoveTo(PaymentStatus.Refunded()))
        {
            PaymentStatusException(PaymentStatus.Refunded());
        }

        Note = "Order marked for refund.";
        RaiseDomainEvent(new PaymentRefundedEvent(this, Money.Of(GetTotal())));
    }

    private void PaymentStatusException(PaymentStatus status) =>
        throw new DomainException("Invalid payment status change attempt from {0} to {1}", PaymentStatus, status);

    private void OrderStatusException(OrderStatus status) =>
        throw new DomainException("Invalid order status change attempt from {0} to {1}", OrderStatus, status);
}
