namespace eCommerceWeb.Domain.Primitives.Entities;

public interface ISoftDeleteEntity
{
    bool IsDeleted { get; }
    string DeletedBy { get; }
    DateTime DeletedOnUtc { get; }
}
