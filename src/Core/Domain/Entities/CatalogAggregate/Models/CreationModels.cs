namespace eCommerceWeb.Domain.Entities.CatalogAggregate;

public readonly record struct CategoryCreationModel(string Name, string UrlSlug);

public readonly record struct ProductCreationModel(
    string Sku,
    string Name,
    string? Description,
    bool OnSale,
    decimal UnitPrice,
    decimal SalePrice,
    int? StockQuantity,
    bool HasUnlimitedStock,
    bool IsFeatured,
    bool EnableProductReviews,
    bool EnableRelatedProducts,
    string UrlSlug,
    string? MetaTitle,
    string? MetaKeywords,
    string? MetaDescription,
    string? SocialImageUrl
);
