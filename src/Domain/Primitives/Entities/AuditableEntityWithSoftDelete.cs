namespace eCommerceWeb.Domain.Primitives.Entities;

public abstract class AuditableEntityWithSoftDelete<TId> : AuditableEntity<TId>, ISoftDeleteEntity
{
    public bool IsDeleted { get; private set;}

    public void ToggleIsDeleted(bool isDeleted) => IsDeleted = isDeleted;
}
