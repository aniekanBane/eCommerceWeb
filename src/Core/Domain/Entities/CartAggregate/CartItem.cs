using eCommerceWeb.Domain.Primitives.Entities;
using eCommerceWeb.Domain.ValueObjects;

namespace eCommerceWeb.Domain.Entities.CartAggregate;

public sealed class CartItem : Entity<int>
{   
    #pragma warning disable CS8618
    private CartItem() {} // EF Core

    public CartItem(string productId, int quantity, Money unitPrice)
    {
        ProductId = productId;
        UnitPrice = unitPrice;
        SetQuantity(quantity);
    }

    public CartItem(string productId, int quantity, decimal unitPrice) 
        : this(productId, quantity, Money.Of(unitPrice)) {}

    public string ProductId { get; private set; }
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; }

    public void AddQuantity(int quantity)
    {
        Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);
        Quantity += quantity;
    }

    public void SetQuantity(int quantity)
    {
        Guard.Against.OutOfRange(quantity, nameof(quantity), 0, int.MaxValue);
        Quantity = quantity;
    }
}
