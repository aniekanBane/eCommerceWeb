namespace eCommerceWeb.Domain.Entities.CatalogAggregate;

public sealed record class ProductImage
{
    public bool IsThumbnail { get; init; }
    public string? AltName { get; init; }
    public required string Uri { get; init; }
    public required int DisplayOrder { get; init ; } 
}
