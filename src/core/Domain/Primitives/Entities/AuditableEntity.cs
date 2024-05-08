using System.ComponentModel.DataAnnotations;

namespace eCommerceWeb.Domain.Primitives.Entities;

public abstract class AuditableEntity<TId> : Entity<TId>, IAuditableEntity
{
    [Timestamp]
    public uint Version { get; private set; }

    [MaxLength(256)]
    public string Author { get; private set; } = string.Empty;
    public DateTime CreatedOnUtc { get; private set; }
    
    [MaxLength(256)]
    public string Editor { get; private set; } = string.Empty;
    public DateTime LastModifiedOnUtc { get; private set; }
}


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public sealed class NotAuditableAttribute : Attribute;
