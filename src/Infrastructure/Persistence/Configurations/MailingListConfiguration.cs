using eCommerceWeb.Domain.Entities;
using eCommerceWeb.Domain.Entities.MarketingAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerceWeb.Persistence.Configurations;

internal sealed class MailingListConfiguration : IEntityTypeConfiguration<MailingList>
{
    public void Configure(EntityTypeBuilder<MailingList> builder)
    {
        builder.HasKey(m => m.Id);

        builder.HasIndex(m => m.NormalizedName)
            .IsUnique()
            .IncludeProperties(m => new { m.Name });
        
        /***** Relationships *****/

        builder.HasMany<Subcriber>()
            .WithMany()
            .UsingEntity<MailingListSubcriber>(
                j => j.Property(ml => ml.SubcribedOnUtc).HasDefaultValueSql("now()")
            );
    }
}
