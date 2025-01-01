using eCommerceWeb.Domain.Primitives.Entities;
using eCommerceWeb.Domain.ValueObjects;

namespace eCommerceWeb.Domain.Entities.OrderAggregate;

public sealed class OrderItem : Entity<int>
{
    internal OrderItem(ProductOrdered productOrdered, Money unitPrice, int quantity )
    {
        ProductOrdered = productOrdered;
        UnitPrice = unitPrice;
        Quantity = Guard.Against.NegativeOrZero(quantity, nameof(quantity));
    }

    #pragma warning disable CS8618
    private OrderItem() { } // EF Core
    #pragma warning restore CS8618

    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; }
    public ProductOrdered ProductOrdered { get; private set; }

    public void AddQuantity(int quantity)
    {
        Quantity += Guard.Against.Negative(quantity, nameof(quantity)); 
    }
}
