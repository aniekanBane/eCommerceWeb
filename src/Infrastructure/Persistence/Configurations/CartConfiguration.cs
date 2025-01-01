using eCommerceWeb.Domain.Entities.CartAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerceWeb.Persistence.Configurations;

internal sealed class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.HasKey(c => c.Id);

        /***** Relationships *****/

        builder.HasMany(c => c.CartItems)
            .WithOne()
            .HasForeignKey(ci => ci.CartId);
    }
}

internal sealed class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasKey(ci => ci.Id);
    }
}
