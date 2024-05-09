using eCommerceWeb.Domain.Primitives.Entities;

namespace eCommerceWeb.Domain.Entities.CatalogAggregate;

public sealed class ProductReview : Entity<int>
{
    public Guid CustomerId { get; private set; }
    public string ProductId { get; private set; }
    public string Title { get; private set; }
    public string ReviewText { get; private set; }
    public int Rating { get; private set; }

    public ProductReview(
        Guid customerId, string productId, 
        string title, string reviewText, int rating)
    {
        Guard.Against.NullOrDefault(customerId, nameof(customerId));
        Guard.Against.NullOrWhiteSpace(productId, nameof(productId));
        Guard.Against.NullOrWhiteSpace(title, nameof(title));
        Guard.Against.OutOfRange(rating, nameof(rating), 1, 5);

        CustomerId = customerId;
        ProductId = productId;
        Title = title;
        ReviewText = reviewText;
        Rating = rating;
    }

    #pragma warning disable CS8618
    private ProductReview() { } // EF Core
}
