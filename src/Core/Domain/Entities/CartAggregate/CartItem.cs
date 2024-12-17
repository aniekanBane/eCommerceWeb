using eCommerceWeb.Domain.Primitives.Entities;
using eCommerceWeb.Domain.ValueObjects;

namespace eCommerceWeb.Domain.Entities.CartAggregate;

public sealed class CartItem : Entity<int>
{   
    internal CartItem(string productId, Money unitPrice, int quantity)
    {
        ProductId = productId;
        UnitPrice = unitPrice;
        SetQuantity(quantity);
    }

    #pragma warning disable CS8618
    private CartItem() {} // EF Core
    #pragma warning restore CS8618

    public int CartId { get; private set; }
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
