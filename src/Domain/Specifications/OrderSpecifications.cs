using Ardalis.Specification;
using eCommerceWeb.Domain.Entities.OrderAggregate;

namespace eCommerceWeb.Domain.Specifications;

public class OrderWithItemsByIdSpec : Specification<Order>
{
    public OrderWithItemsByIdSpec(Guid orderId)
    {
        Query
            .Where(o => o.Id == orderId)
            .Include(o => o.OrderItems);
    }
}

public class CustomerOrderWithItemsSpec : Specification<Order>
{
    public CustomerOrderWithItemsSpec(Guid customerId)
    {
        Query.Where(o => o.CustomerId == customerId)
            .Include(o => o.OrderItems);
    }
}
