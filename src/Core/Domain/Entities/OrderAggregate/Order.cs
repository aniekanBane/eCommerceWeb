using eCommerceWeb.Domain.Events;
using eCommerceWeb.Domain.Primitives.Entities;
using eCommerceWeb.Domain.ValueObjects;

namespace eCommerceWeb.Domain.Entities.OrderAggregate;

public sealed class Order : AuditableEntityWithDomainEvent<Guid>, IAggregateRoot
{   
    #pragma warning disable CS8618
    private Order() { } // EF Core
    #pragma warning restore CS8618

    public Order(Guid customerId, Address billingAddress, Address deliveryAddress) 
    {   
        Id = Guid.NewGuid();
        CustomerId = customerId;
        BillingAddress = billingAddress;
        DeliveryAddress = deliveryAddress;
        OrderStatus = OrderStatus.Pending();
        PaymentStatus = PaymentStatus.Pending();
    }

    public Guid CustomerId { get; private set; }
    public string OrderNo { get; private set; } = string.Empty;
    public OrderStatus OrderStatus { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; }
    public Address BillingAddress { get; private set; }
    public Address DeliveryAddress { get; private set; }
    public string? Note { get; private set; }

    private readonly List<OrderItem> _orderItems = [];
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public void AddItem(ProductOrdered productOrdered, Money unitPrice, int quantity = 1) 
    { 
        var existingItem = _orderItems.SingleOrDefault(i => i.ProductOrdered.ProductId == productOrdered.ProductId);
        if (existingItem is null)
        {
            var orderItem = new OrderItem(productOrdered, unitPrice, quantity);
            _orderItems.Add(orderItem);
            return;
        }
        else
        {
            existingItem.AddQuantity(quantity);
        }

    }

    public decimal GetTotal() => _orderItems.Sum(x => x.Quantity * x.UnitPrice.Amount);

    public void SetFulfilledStatus()
    {
        OrderStatus = OrderStatus.MoveTo(OrderStatus.Fulfilled());
        RaiseDomainEvent(new OrderFulfilledEvent(this));
        Note = "Order has been shipped.";
    }

    public void SetCancelledStatus()
    {
        OrderStatus = OrderStatus.MoveTo(OrderStatus.Cancelled());
        RaiseDomainEvent(new OrderCancelledEvent(this));
        Note = "Order has been cancelled.";
    }

    public void SetPaymentFailedStatus()
    {
        PaymentStatus = PaymentStatus.MoveTo(PaymentStatus.Failed());
        SetCancelledStatus();
        Note = "Payment failed.";
    }

    public void SetPaymentPaidStatus()
    {
        Guard.Against.InvalidInput(OrderStatus, nameof(OrderStatus), os => os == OrderStatus.Pending());
        
        PaymentStatus = PaymentStatus.MoveTo(PaymentStatus.Paid());
        RaiseDomainEvent(new OrderPaidEvent(this));
        Note = "Order has been paid.";
    }

    public void SetRefundedStatus()
    {
        Guard.Against.InvalidInput(OrderStatus, nameof(OrderStatus), os => os != OrderStatus.Cancelled());

        PaymentStatus = PaymentStatus.MoveTo(PaymentStatus.Refunded());
        RaiseDomainEvent(new PaymentRefundedEvent(this, Money.Of(GetTotal())));
        Note = "Order marked for refund.";
    }
}
