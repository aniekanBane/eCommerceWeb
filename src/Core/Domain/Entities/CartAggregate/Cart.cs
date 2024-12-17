using eCommerceWeb.Domain.Primitives.Entities;
using eCommerceWeb.Domain.ValueObjects;

namespace eCommerceWeb.Domain.Entities.CartAggregate;

public sealed class Cart : AuditableEntityWithDomainEvent<int>, IAggregateRoot
{
    public Cart(Guid customerId)
    {
        CustomerId = Guard.Against.Default(customerId, nameof(customerId));
    }

    private Cart() { } // EF Core

    public Guid CustomerId { get; private set; } 

    private readonly List<CartItem> _cartItems = [];
    public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

    public int TotalItems => _cartItems.Sum(i => i.Quantity);

    public void AddItem(string productId, decimal unitPrice, int quantity = 1)
    {
        var existingItem = CartItems.FirstOrDefault(i => i.ProductId == productId);

        if (existingItem is null)
        {
            _cartItems.Add(new(productId, Money.Of(unitPrice), quantity));
            return;
        }

        existingItem.AddQuantity(quantity);
    }

    public void RemoveEmptyItems() => _cartItems.RemoveAll(i => i.Quantity == 0);

    public void SetNewCustomerId(Guid customerId) => CustomerId = customerId;
}
