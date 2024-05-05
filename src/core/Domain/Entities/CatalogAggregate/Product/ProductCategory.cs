namespace eCommerceWeb.Domain.Entities.CatalogAggregate;

public sealed class ProductCategory(int categoryId, string productId)
{
    public int CategoryId { get; private set; } = categoryId;
    public string ProductId { get; private set; } = productId;
}
