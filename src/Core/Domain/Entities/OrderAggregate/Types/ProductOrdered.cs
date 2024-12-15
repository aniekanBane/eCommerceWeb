namespace eCommerceWeb.Domain.Entities.OrderAggregate;

public sealed record class ProductOrdered
{
    public ProductOrdered(string productId, string productSku, string productName, string productImageUri)
    {
        Guard.Against.NullOrWhiteSpace(productId, nameof(productId));
        Guard.Against.NullOrWhiteSpace(productName, nameof(productName));
        Guard.Against.NullOrWhiteSpace(productSku, nameof(productSku));
        Guard.Against.NullOrWhiteSpace(productImageUri, nameof(productImageUri));

        ProductId = productId;
        ProductSku = productSku;
        ProductName = productName;
        ProductImageUri = productImageUri;
    }

    #pragma warning disable CS8618
    private ProductOrdered() { } // EF Core
    #pragma warning restore CS8618

    public string ProductId { get; init; }
    public string ProductSku { get; init; }
    public string ProductName { get; init; }
    public string ProductImageUri { get; init; }
}
