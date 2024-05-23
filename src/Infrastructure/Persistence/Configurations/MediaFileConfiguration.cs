using eCommerceWeb.Domain.Entities.Misc;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Constants;

namespace eCommerceWeb.Persistence.Configurations;

internal sealed class MediaFileConfiguration : IEntityTypeConfiguration<MediaFile>
{
    public void Configure(EntityTypeBuilder<MediaFile> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedNever();

        builder.Property(m => m.Title).HasMaxLength(DomainModelConstants.SEO_IMAGE_ALT_MAX_LENGTH);
        builder.Property(m => m.Description).HasMaxLength(DomainModelConstants.SEO_IMAGE_DESC_MAX_LENGTH);

        builder.Property(m => m.FileType)
            .HasConversion(v => v.Value, v => new FileType(v))
            .IsRequired();

        // ----- Table Configurations -----

        builder.ToTable(b =>
        {
            b.HasCheckConstraint("ck_file_type", $"file_type IN ('{string.Join("', '", FileType.ListNames())}')");
            b.HasCheckConstraint("ck_file_size", $"file_size <= {DomainModelConstants.MEDIA_FILE_MAX_SIZE}");
        });
    }
}
