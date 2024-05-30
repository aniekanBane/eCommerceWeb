using eCommerceWeb.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Constants;

namespace eCommerceWeb.Persistence.Configurations;

internal static class ComplexObjectConfiguration
{
    public static void ConfigureAddress(ComplexPropertyBuilder<Address> builder)
    {
        builder.IsRequired();
        builder.Property(a => a.Line1).HasMaxLength(DomainModelConstants.ADDRESS_LINE_MAX_LENGTH);
        builder.Property(a => a.Line2).HasMaxLength(DomainModelConstants.ADDRESS_LINE_MAX_LENGTH);
        builder.Property(a => a.City).HasMaxLength(DomainModelConstants.CITY_MAX_LENGTH);
        builder.Property(a => a.StateProvince).HasMaxLength(DomainModelConstants.CITY_MAX_LENGTH);
        builder.Property(a => a.ZipCode).HasMaxLength(DomainModelConstants.ZIPCODE_MAX_LENGTH);
        builder.Property(a => a.Country).HasMaxLength(DomainModelConstants.COUNTRY_NAME_MAX_LENGTH);
    } 

    public static void ConfigureSeo(ComplexPropertyBuilder<Seo> builder)
    { 
        builder.IsRequired();
        builder.Property(s => s.UrlSlug).IsRequired();
        builder.Property(s => s.MetaTitle).HasMaxLength(DomainModelConstants.SEO_TITLE_MAX_LENGTH);
        builder.Property(s => s.MetaKeywords).HasMaxLength(DomainModelConstants.SEO_KEYWORDS_MAX_LENGTH);
        builder.Property(s => s.MetaDescription).HasMaxLength(DomainModelConstants.SEO_DESC_MAX_LENGTH);
    }

    public static void ConfigureName<T>(OwnedNavigationBuilder<T, Name> builder) where T : class
    {
        builder.WithOwner();
        builder.Property(n => n.Firstname).HasMaxLength(DomainModelConstants.NAME_MAX_LENGTH).HasColumnName("firstname");
        builder.Property(n => n.Lastname).HasMaxLength(DomainModelConstants.NAME_MAX_LENGTH).HasColumnName("lastname");
        builder.Property(n => n.Fullname)
            .HasColumnName("fullname")
            .HasComputedColumnSql("firstname || ' ' || lastname", true);
        builder.HasIndex(n => n.Fullname);
    }
}
