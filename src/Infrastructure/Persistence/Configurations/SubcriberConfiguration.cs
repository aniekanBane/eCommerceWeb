using eCommerceWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerceWeb.Persistence.Configurations;

internal sealed class SubcriberConfiguration : IEntityTypeConfiguration<Subcriber>
{
    public void Configure(EntityTypeBuilder<Subcriber> builder)
    {
        builder.HasKey(s => s.Id);

        builder.HasIndex(s => s.EmailAddress).IsUnique();
        
        builder.OwnsOne(s => s.Name, ComplexObjectConfiguration.ConfigureName);
    }
}
