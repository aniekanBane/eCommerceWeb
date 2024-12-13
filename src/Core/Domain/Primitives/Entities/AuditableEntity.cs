using System.ComponentModel.DataAnnotations;

namespace eCommerceWeb.Domain.Primitives.Entities;

public abstract class AuditableEntity<TId> : Entity<TId>, IAuditableEntity
{
    [Timestamp]
    public uint Version { get; private set; }

    [MaxLength(256)]
    public string CreatedBy { get; private set; } = string.Empty;
    public DateTime CreatedOnUtc { get; private set; }
    
    [MaxLength(256)]
    public string LastModifiedBy { get; private set; } = string.Empty;
    public DateTime LastModifiedOnUtc { get; private set; }
}
