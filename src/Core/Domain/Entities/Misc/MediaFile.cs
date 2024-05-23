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

    public MediaFile(long fileSize, string fileType, string fileName, string contentType) 
        : this(FileType.Of(fileType), fileSize, fileName, contentType) { }
    
    private MediaFile(FileType fileType, long fileSize, string fileName, string contentType)
    {
        Guard.Against.NullOrWhiteSpace(fileName, nameof(fileName));
        Guard.Against.NullOrWhiteSpace(contentType, nameof(contentType));
        Guard.Against.OutOfRange(fileSize, nameof(fileSize), 0, DomainModelConstants.MEDIA_FILE_MAX_SIZE);
        Guard.Against.InvalidInput(
            fileName, 
            nameof(fileName), 
            x => fileType.AllowedExtensions().Split(' ').Contains(Path.GetExtension(x)),
            ErrorMessages.FileEntry.InvalidFile
        );

        Id = Guid.NewGuid();
        FileSize = fileSize;
        FileName = fileName;
        ContentType = contentType;
        FileType = fileType;
        FileLocation = string.Empty;
    }

    #pragma warning disable CS8618
    private MediaFile() {}

    public void SetLocation(string location)
    {
        Guard.Against.NullOrWhiteSpace(location, nameof(location));
        FileLocation = location;
    }

    public MediaFile Update(string? title, string? description)
    {
        if (!string.IsNullOrWhiteSpace(title))
            Guard.Against.StringTooLong(title, DomainModelConstants.SEO_IMAGE_ALT_MAX_LENGTH, nameof(title));
        if (!string.IsNullOrWhiteSpace(description))
            Guard.Against.StringTooLong(description, DomainModelConstants.SEO_IMAGE_DESC_MAX_LENGTH, nameof(description));

        Title = title;
        Description = description;
        return this;
    }
}
