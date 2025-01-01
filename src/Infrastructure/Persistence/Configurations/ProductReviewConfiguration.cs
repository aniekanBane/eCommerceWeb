using eCommerceWeb.Domain.Entities.CatalogAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerceWeb.Persistence.Configurations;

public class ProductReviewConfiguration : IEntityTypeConfiguration<ProductReview>
{
    public void Configure(EntityTypeBuilder<ProductReview> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.CustomerId).IsRequired();
        
        builder.Property(p => p.Title).HasMaxLength(128);

        // ----- Table Configurations -----

        builder.ToTable(b =>
        {
            b.HasCheckConstraint("ck_rating", "rating BETWEEN 1 AND 5");
        });
    }
}
