namespace eCommerceWeb.Domain.Primitives.Entities;

public interface IAuditableEntity
{
    uint Version { get; }
    string CreatedBy { get; }
    DateTime CreatedOnUtc { get; }
    string LastModifiedBy { get; }
    DateTime LastModifiedOnUtc { get; }
}
