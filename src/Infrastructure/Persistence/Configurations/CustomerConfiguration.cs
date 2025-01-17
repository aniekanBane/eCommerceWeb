using eCommerceWeb.Domain.Entities.CustomerAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerceWeb.Persistence.Configurations;

internal sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.HasIndex(c => c.EmailAddress).IsUnique();
        
        builder.HasIndex(c => c.PhoneNumber);

        builder.OwnsOne(c => c.Name, ComplexObjectConfiguration.ConfigureName);
    }
}
