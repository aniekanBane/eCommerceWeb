namespace eCommerceWeb.Domain.Primitives.Entities;

public interface IAuditableEntity
{
    uint Version { get; }
    string Author { get; }
    DateTime CreatedOnUtc { get; }
    string Editor { get; }
    DateTime LastModifiedOnUtc { get; }
}
