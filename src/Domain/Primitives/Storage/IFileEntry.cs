namespace eCommerceWeb.Domain.Primitives.Storage;

public interface IFileEntry
{
    Guid Id { get; }
    string Location { get; }
    string FileName { get; }
}
