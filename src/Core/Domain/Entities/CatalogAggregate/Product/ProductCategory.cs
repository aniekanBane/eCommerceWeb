namespace eCommerceWeb.Domain.Entities.CatalogAggregate;

public sealed class ProductCategory
{
    public ProductCategory(int categoryId, string productId)
    {
        CategoryId = categoryId;
        ProductId = productId;
    }

    #pragma warning disable CS8618
    private ProductCategory() { } // EF Core
    #pragma warning restore CS8618

    public int CategoryId { get; private set; }
    public string ProductId { get; private set; }
}
