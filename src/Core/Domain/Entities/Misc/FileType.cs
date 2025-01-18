using Ardalis.SmartEnum;

namespace eCommerceWeb.Domain.Entities.Misc;

public sealed record class FileType
{
    private FileTypeEnum _type = null!;

    private FileType() { } // EF Core

    public FileType(string value)
    {
        Value = Guard.Against.NullOrWhiteSpace(value);
    }

    public string Value 
    {
        get => _type.Name;
        private init 
        { 
            Guard.Against.InvalidInput(
                value, 
                nameof(value),
                predicate: x => FileTypeEnum.TryFromName(x, true, out _type),
                ErrorMessages.FileEntry.InvalidFileType
            );
        }
    }

    public string AllowedExtensions() => _type.AllowedExtensions;

    public static implicit operator string(FileType fileType) => fileType.Value;

    public static FileType Of(string value) => new(value);
    public static List<string> ListNames() => [.. FileTypeEnum.List.Select(x => x.Name)];

    public static FileType Document() => new(FileTypeEnum.Document.Name);
    public static FileType Image() => new(FileTypeEnum.Image.Name);
    public static FileType Video() => new(FileTypeEnum.Video.Name);

    private abstract class FileTypeEnum(string name, ushort value) 
        : SmartEnum<FileTypeEnum, ushort>(name, value)
    {
        public static readonly FileTypeEnum Image = new ImageType();
        public static readonly FileTypeEnum Video = new VideoType();
        public static readonly FileTypeEnum Document = new DocumentType();

        public abstract string AllowedExtensions { get; }

        private sealed class DocumentType() : FileTypeEnum("Document", 1)
        {
            public override string AllowedExtensions => ".pdf .csv .doc .docx .xlsx";
        }

        private sealed class ImageType() : FileTypeEnum("Image", 2)
        {
            public override string AllowedExtensions => ".jpg .jpeg .png .svg .webp";
        }

        private sealed class VideoType() : FileTypeEnum("Video", 3)
        {
            public override string AllowedExtensions => ".mp3 .mp4 .mov .webm";
        }
    }
}
