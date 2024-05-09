namespace eCommerceWeb.Domain.Entities.CatalogAggregate;

public readonly record struct CategoryUpdateModel(
    string Name,
    bool IsEnabled,
    bool IsVisible,
    string UrlSlug,
    string? MetaTitle,
    string? MetaKeywords,
    string? MetaDescription
);

public readonly record struct ProductUpdateModel(
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
