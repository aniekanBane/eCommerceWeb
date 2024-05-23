using eCommerceWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Constants;

namespace eCommerceWeb.Persistence.Configurations;

internal sealed class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.Property(t => t.Name)
            .HasMaxLength(DomainModelConstants.TAG_NAME_MAX_LENGTH)
            .UseCollation(DbConstants.Collation.CASE_INSENSITIVE_COLLATION);
        builder.HasIndex(t => t.Name);
    }
}
