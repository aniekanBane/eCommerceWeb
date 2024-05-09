using eCommerceWeb.Domain.Primitives.Entities;
using eCommerceWeb.Domain.ValueObjects;

namespace eCommerceWeb.Domain.Entities.CartAggregate;

public sealed class Cart : AuditableEntityWithDomainEvent<int>, IAggregateRoot
{
    private Cart() { } // EF Core

    public Cart(Guid customerId)
    {
        Guard.Against.NullOrDefault(customerId, nameof(customerId));
        CustomerId = customerId;
    }

    public Guid CustomerId { get; private set; } 

    private readonly List<CartItem> _cartItems = [];
    public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

    [NotAuditable]
    public int TotalItems => _cartItems.Sum(i => i.Quantity);

    public void AddItem(string productId, decimal unitPrice, int quantity = 1)
    {
        var existingItem = CartItems.FirstOrDefault(i => i.ProductId == productId);

        if (existingItem is null)
        {
            _cartItems.Add(new(productId, quantity, Money.Of(unitPrice)));
            return;
        }

        existingItem.AddQuantity(quantity);
    }

    public void RemoveEmptyItems() => _cartItems.RemoveAll(i => i.Quantity == 0);

    public void SetNewCustomerId(Guid customerId) => CustomerId = customerId;
}
