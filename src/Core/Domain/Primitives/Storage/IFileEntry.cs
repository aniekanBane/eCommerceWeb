namespace eCommerceWeb.Domain.Primitives.Storage;

public interface IFileEntry
{
    Guid Id { get; }
    string ContentType { get; }
    string FileLocation { get; }
    string FileName { get; }
}
