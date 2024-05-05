using eCommerceWeb.Domain.ValueObjects;

namespace eCommerceWeb.Domain.Entities.OrderAggregate;

public interface IOrderService
{
    Task CreateOrderAsync(
        Guid cartId, 
        Address shippingAddress, 
        Address billingAddress,
        CancellationToken cancellationToken = default
    );
}
