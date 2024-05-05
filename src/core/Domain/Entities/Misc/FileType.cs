using Ardalis.SmartEnum;

namespace eCommerceWeb.Domain.Entities.Misc;

public sealed record class FileType
{
    private FileTypeEnum _type = null!;
    public string Value 
    {
        get => _type.Name;
        private init { _type = Parse(value); }
    }

    public FileType(string value)
    {
        Value = value;
    }

    public string AllowedExtensions() => _type.AllowedExtensions;

    public static implicit operator string(FileType fileType) => fileType.Value;

    public static FileType Of(string value) => new(value);
    public static List<string> ListNames() => FileTypeEnum.List.Select(x => x.Name).ToList();

    public static FileType Document() => new(FileTypeEnum.Document.Name);
    public static FileType Image() => new(FileTypeEnum.Image.Name);
    public static FileType Video() => new(FileTypeEnum.Video.Name);

    private static FileTypeEnum Parse(string value)
    {
        Guard.Against.NullOrWhiteSpace(value, nameof(value));
        var success = FileTypeEnum.TryFromName(value, true, out var result);
        Guard.Against.Expression(x => !x, success, ErrorMessages.FileEntry.InvalidFileType);
        return result;
    }

    private FileType() { } // EF Core

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
            public override string AllowedExtensions => ".png .jpg .jpeg .png .svg .webp";
        }

        private sealed class VideoType() : FileTypeEnum("Video", 3)
        {
            public override string AllowedExtensions => ".mp3 .mp4 .mov .webm";
        }
    }
}
