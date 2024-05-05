using eCommerceWeb.Domain.Primitives.Entities;
using eCommerceWeb.Domain.ValueObjects;

namespace eCommerceWeb.Domain.Entities.OrderAggregate;

public sealed class OrderItem : Entity<int>
{
    #pragma warning disable CS8618
    private OrderItem() {}

    public OrderItem(int quantity, Money unitPrice, ProductOrdered productOrdered)
    {
        ProductOrdered = productOrdered;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public OrderItem(int quantity, decimal unitPrice, ProductOrdered productOrdered) 
        : this(quantity, Money.Of(unitPrice), productOrdered) {}

    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; }
    public ProductOrdered ProductOrdered { get; private set; }
}
