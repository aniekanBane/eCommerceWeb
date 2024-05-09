using eCommerceWeb.Domain.Primitives.Entities;
using eCommerceWeb.Domain.ValueObjects;

namespace eCommerceWeb.Domain.Entities.CatalogAggregate;

public sealed class Product : AuditableEntityWithDomainEvent<string>, IAggregateRoot
{
    #region properties

    /* Details */
    public string Name { get; private set; }
    public string NormalisedName 
    { 
        get => Name.ToUpper(); 
        private set {}
    }
    public string? Description { get; private set; }
    public string Sku { get; private set; }
    public string NormalisedSku 
    {
        get => Sku.ToUpper();
        private set {}
    }

    /* Images */
    public string? ThumbnailUri
    { 
        get => Images.FirstOrDefault(i => i.IsThumbnail)?.Uri; 
        private set {} 
    }
    private readonly List<ProductImage> _images = [];
    public IReadOnlyCollection<ProductImage> Images => _images.AsReadOnly();

    /* Price */
    public bool OnSale { get; private set; }
    public Money UnitPrice { get; private set; }
    public Money SalePrice { get; private set; }

    /* Stock */
    public int? StockQuantity { get; private set; }
    public bool HasUnlimitedStock { get; private set; }

    /* Organization */
    public Visibility Visibility { get; private set; }
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
    private Product() {} // EF Core

    public Product(ProductCreationModel creationModel)
    {
        Id = Guid.NewGuid().ToString();
        Visibility = Visibility.Hidden();
        IsFeatured = creationModel.IsFeatured;
        EnableProductReviews = creationModel.EnableProductReviews;
        EnableRelatedProducts = creationModel.EnableRelatedProducts;
        Seo = new(
            creationModel.UrlSlug, creationModel.MetaTitle, 
            creationModel.MetaKeywords, creationModel.MetaDescription
        );
        SocialImageUrl = creationModel.SocialImageUrl;
        SetDetails(creationModel.Sku, creationModel.Name, creationModel.Description);
        SetPrice(creationModel.OnSale, creationModel.UnitPrice, creationModel.SalePrice);
        SetStock(creationModel.HasUnlimitedStock, creationModel.StockQuantity);
    }

    #endregion

    #region methods

    public Product AddCategory(Category category)
    {
        _categories.Add(category);
        return this;
    }

    public Product AddImage(ProductImage image)
    {
        _images.Add(image);
        return this;
    }

    public Product AddRelatedProduct(Product product)
    {
        _relatedProducts.Add(product);
        return this;
    }

    public Product AddTag(ProductTag tag)
    {
        _tags.Add(tag);
        return this;
    }

    public Product SetVisibility(string visibility)
    {
        Visibility = Visibility.Of(visibility); // TODO: Add Schedule Implementation
        
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
        SetDetails(updateModel.Sku, updateModel.Name, updateModel.Description);
        SetPrice(updateModel.OnSale, updateModel.UnitPrice, updateModel.SalePrice);
        SetStock(updateModel.HasUnlimitedStock, updateModel.StockQuantity);
        return this;
    }

    private void SetDetails(string sku, string name, string? description)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Guard.Against.StringTooLong(name, DomainModelConstants.PRODUCT_NAME_MAX_LENGTH, nameof(name));

        Guard.Against.NullOrWhiteSpace(sku, nameof(sku));
        Guard.Against.InvalidStringLength(DomainModelConstants.PRODUCT_SKU_LENGTH, sku, nameof(sku));

        if (!string.IsNullOrWhiteSpace(description))
            Guard.Against.StringTooLong(description, DomainModelConstants.PRODUCT_DESC_MAX_LENGTH, nameof(description));

        Sku = sku;
        Name = name;
        Description = description;
    }

    private void SetPrice(bool onSale, decimal unitPrice, decimal salePrice)
    {
        UnitPrice = Money.Of(Guard.Against.Negative(unitPrice, nameof(unitPrice)));
        SalePrice = Money.Zero;

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
