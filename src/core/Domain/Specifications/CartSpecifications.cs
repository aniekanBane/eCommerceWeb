using Ardalis.Specification;
using eCommerceWeb.Domain.Entities.CartAggregate;

namespace eCommerceWeb.Domain.Specifications;

public class CartWithItemsSpec : Specification<Cart>
{
    public CartWithItemsSpec(int cartId)
    {
        Query.Where(c => c.Id == cartId)
            .Include(c => c.CartItems);
    }

    public CartWithItemsSpec(Guid customerId)
    {
        Query.Where(c => c.CustomerId == customerId)
            .Include(c => c.CartItems);
    }
}