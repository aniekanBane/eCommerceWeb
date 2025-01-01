namespace eCommerceWeb.Domain.Entities.OrderAggregate;

public sealed record class ProductOrdered
{
    public ProductOrdered(string productId, string productSku, string productName, string productImageUri)
    {
        ProductId = Guard.Against.NullOrWhiteSpace(productId, nameof(productId));
        ProductSku = Guard.Against.NullOrWhiteSpace(productSku, nameof(productSku));
        ProductName = Guard.Against.NullOrWhiteSpace(productName, nameof(productName));
        ProductImageUri = Guard.Against.NullOrWhiteSpace(productImageUri, nameof(productImageUri));
    }

    #pragma warning disable CS8618
    private ProductOrdered() { } // EF Core
    #pragma warning restore CS8618

    public string ProductId { get; init; }
    public string ProductSku { get; init; }
    public string ProductName { get; init; }
    public string ProductImageUri { get; init; }
}
