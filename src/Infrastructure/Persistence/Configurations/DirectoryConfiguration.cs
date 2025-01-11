using eCommerceWeb.Domain.Entities.Directory;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerceWeb.Persistence.Configurations;

internal sealed class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasIndex(c => c.NormalizedName)
            .IsUnique()
            .IncludeProperties(c => new { c.Name });

        builder.HasIndex(c => c.Code).IsUnique();
    }
}

internal sealed class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasIndex(c => c.NormalizedName)
            .IsUnique()
            .IncludeProperties(c => new { c.Name });

        builder.HasIndex(c => c.Cca2).IsUnique();
        builder.HasIndex(c => c.Cca3).IsUnique();
        builder.HasIndex(c => c.Ccn3).IsUnique();

        /***** Relationships *****/

        builder.HasMany(c => c.StateProvinces)
            .WithOne()
            .HasForeignKey(s => s.CountryId);
    }
}

internal sealed class StateProvinceConfiguration : IEntityTypeConfiguration<StateProvince>
{
    public void Configure(EntityTypeBuilder<StateProvince> builder)
    {
        builder.HasKey(s => s.Id);

        builder.HasIndex(s => new { s.CountryId, s.NormalizedName })
            .IsUnique()
            .IncludeProperties(s => new { s.Name });
    }
}
