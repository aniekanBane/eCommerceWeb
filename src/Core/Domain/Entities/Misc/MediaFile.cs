using eCommerceWeb.Domain.Primitives.Entities;
using eCommerceWeb.Domain.Primitives.Storage;

namespace eCommerceWeb.Domain.Entities.Misc;

public sealed class MediaFile : AuditableEntityWithSoftDelete<Guid>, IFileEntry, IAggregateRoot
{
    public string? Title { get; private set; }
    public string? Description { get; private set; }
    public long FileSize { get; private set; }
    public FileType FileType { get; private set; }
    public string FileLocation { get; private set; }
    public string FileName { get; private set; }
    public string ContentType { get; private set; }
    
    public MediaFile(FileType fileType, string fileName, string contentType, long fileSize)
    {
        FileType = fileType;
        FileName = Guard.Against.NullOrWhiteSpace(fileName, nameof(fileName));
        ContentType = Guard.Against.NullOrWhiteSpace(contentType, nameof(contentType));
        FileSize = Guard.Against.OutOfRange(fileSize, nameof(fileSize), 0, DomainModelConstants.MEDIA_FILE_MAX_SIZE);
        Guard.Against.InvalidInput(
            fileName, 
            nameof(fileName), 
            x => fileType.AllowedExtensions().Split(' ').Contains(Path.GetExtension(x)),
            ErrorMessages.FileEntry.InvalidFile
        );

        Id = Guid.NewGuid();
        FileLocation = string.Empty;
    }

    #pragma warning disable CS8618
    private MediaFile() { } // EF Core
    #pragma warning restore CS8618

    public MediaFile SetLocation(string location)
    {
        FileLocation = Guard.Against.NullOrWhiteSpace(location);
        return this;
    }

    public MediaFile Update(string? title, string? description)
    {
        if (!string.IsNullOrWhiteSpace(title))
            Title = Guard.Against.StringTooLong(title, DomainModelConstants.SEO_IMAGE_ALT_MAX_LENGTH);
        if (!string.IsNullOrWhiteSpace(description))
            Description = Guard.Against.StringTooLong(description, DomainModelConstants.SEO_IMAGE_DESC_MAX_LENGTH);

        return this;
    }
}
