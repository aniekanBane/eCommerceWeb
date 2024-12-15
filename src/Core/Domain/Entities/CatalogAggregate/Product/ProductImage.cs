namespace eCommerceWeb.Domain.Entities.CatalogAggregate;

public sealed record class ProductImage
{
    public ProductImage(string uri, string? altName, bool isThumbnail, int displayOrder)
    {
        Uri = uri;
        AltName = altName;
        IsThumbnail = isThumbnail;
        DisplayOrder = displayOrder;
    }

    #pragma warning disable CS8618
    private ProductImage() { } // EF Core
    #pragma warning restore CS8618

    public string Uri { get; init; }
    public string? AltName { get; init; }
    public bool IsThumbnail { get; init; }
    public int DisplayOrder { get; init; }
}
