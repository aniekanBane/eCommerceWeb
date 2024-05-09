using eCommerceWeb.Domain.Entities.CartAggregate;
using eCommerceWeb.Domain.Primitives.Events;

namespace eCommerceWeb.Domain.Events;

public sealed class CartClearedEvent(ImmutableList<CartItem> cartItems) : DomainEvent
{
    public ImmutableList<CartItem> CartItems { get; } = cartItems;
}
