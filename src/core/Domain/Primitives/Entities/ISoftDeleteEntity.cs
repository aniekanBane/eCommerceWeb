namespace eCommerceWeb.Domain.Primitives.Entities;

public interface ISoftDeleteEntity
{
    bool IsDeleted { get; }
}
