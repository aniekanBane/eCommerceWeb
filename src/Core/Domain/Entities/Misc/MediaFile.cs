using eCommerceWeb.Domain.Primitives.Entities;
using eCommerceWeb.Domain.Primitives.Storage;

namespace eCommerceWeb.Domain.Entities.Misc;

public sealed class MediaFile : AuditableEntityWithSoftDelete<Guid>, IFileEntry, IAggregateRoot
{
    public long FileSize { get; private set; }
    public string? AltName { get; private set; }
    public string? Description { get; private set; }
    public FileType FileType { get; private set; }
    public string Location { get; private set; }
    public string FileName { get; private set; }

    public MediaFile(long fileSize, string fileType, string fileName, string location)
    {
        Guard.Against.NullOrWhiteSpace(fileName, nameof(fileName));
        Guard.Against.NullOrWhiteSpace(location, nameof(location));
        Guard.Against.OutOfRange(fileSize, nameof(fileSize), 0, DomainModelConstants.MEDIA_FILE_MAX_SIZE);
        
        Id = Guid.NewGuid();
        FileSize = fileSize;
        FileName = fileName;
        Location = location;
        FileType = FileType.Of(fileType);
    }

    #pragma warning disable CS8618
    private MediaFile() {}
}
