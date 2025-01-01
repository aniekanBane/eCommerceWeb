using eCommerceWeb.Domain.Primitives.Entities;
using eCommerceWeb.Domain.ValueObjects;

namespace eCommerceWeb.Domain.Entities.CatalogAggregate;

public sealed class Product : AuditableEntityWithDomainEvent<string>, IAggregateRoot
{
    #region properties

    /* Details */
    public string Name { get; private set; } = string.Empty;
    public string NormalizedName { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string Sku { get; private set; } = string.Empty;
    public string NormalizedSku { get; private set; } = string.Empty;

    /* Images */
    public string? ThumbnailUri
    { 
        get => Images.FirstOrDefault(i => i.IsThumbnail)?.Uri; 
        private set { } 
    }
    private readonly List<ProductImage> _images = [];
    public IReadOnlyCollection<ProductImage> Images => _images.AsReadOnly();

    /* Price */
    public bool OnSale { get; private set; }
    public Money UnitPrice { get; private set; } = Money.Zero;
    public Money SalePrice { get; private set; } = Money.Zero;

    /* Stock */
    public int? StockQuantity { get; private set; }
    public bool HasUnlimitedStock { get; private set; }

    /* Organization */
    public Visibility Visibility { get; private set; }
    public DateTime? PublishOn  { get; private set; }
    private readonly List<Category> _categories = [];
    public IReadOnlyCollection<Category> Categories => _categories.AsReadOnly();
    private readonly List<ProductTag> _tags = [];
    public IReadOnlyCollection<ProductTag> Tags => _tags.AsReadOnly();

    /* Selling */
    public bool IsFeatured  { get; private set; }
    public bool EnableProductReviews { get; private set; }
    public bool EnableRelatedProducts { get; private set; }
    private readonly List<Product> _relatedProducts = [];
    public IReadOnlyCollection<Product> RelatedProducts => _relatedProducts.AsReadOnly();

    /* Seo */
    public Seo Seo { get; private set; }
    public string? SocialImageUrl { get; private set; }

    #endregion

    #region constructors

    #pragma warning disable CS8618
    private Product() { } // EF Core
    #pragma warning restore CS8618

    public Product(ProductCreationModel creationModel)
    {
        Id = Guid.NewGuid().ToString("N");
        Visibility = Visibility.Hidden();
        IsFeatured = creationModel.IsFeatured;
        EnableProductReviews = creationModel.EnableProductReviews;
        EnableRelatedProducts = creationModel.EnableRelatedProducts;
        Seo = new(
            creationModel.UrlSlug, creationModel.MetaTitle, 
            creationModel.MetaKeywords, creationModel.MetaDescription
        );
        SocialImageUrl = creationModel.SocialImageUrl;
        SetName(creationModel.Name);
        SetDetails(creationModel.Sku, creationModel.Description);
        SetPrice(creationModel.OnSale, creationModel.UnitPrice, creationModel.SalePrice);
        SetStock(creationModel.HasUnlimitedStock, creationModel.StockQuantity);
    }

    #endregion

    #region methods

    public void AddCategory(Category category)
    {
        _categories.Add(category);
    }

    public void AddImage(ProductImage image)
    {
        _images.Add(image);
    }

    public void AddRelatedProduct(Product product)
    {
        Guard.Against.InvalidInput(product, nameof(product), p => p == this);
        _relatedProducts.Add(product);
    }

    public void AddTag(ProductTag tag)
    {
        _tags.Add(tag);
    }

    public Product SetName(string name)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Guard.Against.StringTooLong(name, DomainModelConstants.PRODUCT_NAME_MAX_LENGTH, nameof(name));
        
        Name = name;
        NormalizedName = name.Trim().ToUpperInvariant();
        return this;
    }

    public Product SetVisibility(string visibility, DateTime? publishOn = default)
    {
        Visibility = Visibility.Of(visibility);
        if (visibility == Visibility.Scheduled())
        {
            PublishOn = Guard.Against.Default(publishOn, nameof(publishOn));
        }
        return this;
    }

    public Product Update(ProductUpdateModel updateModel)
    {
        IsFeatured = updateModel.IsFeatured;
        EnableProductReviews = updateModel.EnableProductReviews;
        EnableRelatedProducts = updateModel.EnableRelatedProducts;
        Seo = new(
            updateModel.UrlSlug, updateModel.MetaTitle, 
            updateModel.MetaKeywords, updateModel.MetaDescription
        );
        SocialImageUrl = updateModel.SocialImageUrl;
        SetName(updateModel.Name);
        SetDetails(updateModel.Sku, updateModel.Description);
        SetPrice(updateModel.OnSale, updateModel.UnitPrice, updateModel.SalePrice);
        SetStock(updateModel.HasUnlimitedStock, updateModel.StockQuantity);
        return this;
    }

    private void SetDetails(string sku, string? description)
    {
        Guard.Against.NullOrWhiteSpace(sku, nameof(sku));
        Guard.Against.InvalidStringLength(DomainModelConstants.PRODUCT_SKU_LENGTH, sku, nameof(sku));

        if (!string.IsNullOrWhiteSpace(description))
            Guard.Against.StringTooLong(description, DomainModelConstants.PRODUCT_DESC_MAX_LENGTH, nameof(description));

        Sku = sku;
        Description = description;
    }

    private void SetPrice(bool onSale, decimal unitPrice, decimal salePrice)
    {
        UnitPrice = Money.Of(Guard.Against.Negative(unitPrice, nameof(unitPrice)));

        if (onSale)
        {
            Guard.Against.Zero(unitPrice, nameof(unitPrice));
            Guard.Against.NegativeOrZero(salePrice, nameof(salePrice));
            Guard.Against.InvalidInput(
                salePrice, 
                nameof(salePrice),
                s => s < unitPrice,
                ErrorMessages.Product.InvalidSalePrice
            );

            SalePrice = Money.Of(salePrice);
        }
        
        OnSale = onSale;
    }

    private void SetStock(bool unlimited, int? quantity)
    {
        if (!unlimited)
        {
            Guard.Against.Null(quantity, nameof(quantity));
            Guard.Against.Negative(quantity.Value, nameof(quantity));

            StockQuantity = quantity;
        }

        HasUnlimitedStock = unlimited;
    }

    #endregion
}
