﻿using eCommerceWeb.Domain.Entities.CatalogAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Constants;

namespace eCommerceWeb.Persistence.Configurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(p => p.Id)
            .ValueGeneratedNever()
            .HasMaxLength(DomainModelConstants.PRODUCT_ID_LENGTH)
            .IsFixedLength();

        builder.Property(p => p.Name)
            .HasMaxLength(DomainModelConstants.PRODUCT_NAME_MAX_LENGTH)
            .UseCollation(DbConstants.Collation.CASE_INSENSITIVE_COLLATION);
        builder.HasIndex(p => p.Name).IsUnique();

        builder.Property(p => p.Sku)
            .HasMaxLength(DomainModelConstants.PRODUCT_SKU_LENGTH)
            .IsFixedLength()
            .UseCollation(DbConstants.Collation.CASE_INSENSITIVE_COLLATION);;
        builder.HasIndex(p => p.Sku).IsUnique();

        builder.Property(p => p.Description).HasMaxLength(DomainModelConstants.PRODUCT_DESC_MAX_LENGTH);

        builder.Property(p => p.Visibility)
            .HasConversion(v => v.Value, v => new Visibility(v))
            .HasDefaultValue(Visibility.Hidden())
            .IsRequired();

        builder.Property(p => p.EnableProductReviews).HasDefaultValue(true);

        builder.ComplexProperty(p => p.Seo, ComplexObjectConfiguration.ConfigureSeo);

        // ----- Relationship Configurations -----

        builder.OwnsMany(p => p.Images, n =>
        {
            n.WithOwner();
        });

        builder.HasMany(p => p.RelatedProducts).WithMany();

        builder.HasMany(p => p.Categories)
            .WithMany()
            .UsingEntity<ProductCategory>();

        builder.HasMany(p => p.Tags).WithOne();

        builder.HasMany<ProductReview>()
            .WithOne().HasForeignKey(p => p.ProductId);

        // ----- Table Configurations -----
        
        builder.ToTable(b =>
        {
            b.HasCheckConstraint("ck_visibility", $"visibility IN ('{string.Join("', '", Visibility.ListNames())}')");
            b.HasCheckConstraint("ck_unit_price_value", "unit_price >= 0");
            b.HasCheckConstraint(
                "ck_stock", 
                "(has_unlimited_stock = FALSE AND stock_quantity >= 0) OR (has_unlimited_stock AND stock_quantity IS NULL)"
            );
            b.HasCheckConstraint(
                "ck_price", 
                "(on_sale = TRUE AND sale_price > 0 AND sale_price < unit_price) OR (on_sale = FALSE AND sale_price = 0)"
            );
        });
    }
}
