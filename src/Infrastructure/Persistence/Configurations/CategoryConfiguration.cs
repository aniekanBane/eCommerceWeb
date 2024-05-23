﻿using eCommerceWeb.Domain.Entities.CatalogAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Constants;

namespace eCommerceWeb.Persistence.Configurations;

internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(c => c.Id).HasIdentityOptions(100221);

        builder.Property(c => c.Name)
            .HasMaxLength(DomainModelConstants.CATEGORY_NAME_MAX_LENGTH)
            .UseCollation(DbConstants.Collation.CASE_INSENSITIVE_COLLATION);
        builder.HasIndex(c => c.Name).IsUnique();

        builder.Property(c => c.IsEnabled).HasDefaultValue(true);
        builder.Property(c => c.IsVisible).HasDefaultValue(true);

        builder.ComplexProperty(c => c.Seo, ComplexObjectConfiguration.ConfigureSeo);

        // ----- Relationship Configurations -----

        builder.HasMany(c => c.SubCategories)
            .WithOne().HasForeignKey(c => c.ParentCategoryId);
    }
}
