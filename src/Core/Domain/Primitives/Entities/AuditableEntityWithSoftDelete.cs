using System.ComponentModel.DataAnnotations;

namespace eCommerceWeb.Domain.Primitives.Entities;

public abstract class AuditableEntityWithSoftDelete<TId> : AuditableEntity<TId>, ISoftDeleteEntity
{
    public bool IsDeleted { get; private set;}
    
    [StringLength(256)]
    public string DeletedBy { get; private set; } = string.Empty;

    public DateTime DeletedOnUtc { get; private set; }
}
