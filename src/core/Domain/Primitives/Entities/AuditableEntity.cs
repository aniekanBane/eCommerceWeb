namespace eCommerceWeb.Domain.Primitives.Entities;

public abstract class AuditableEntity<TId> : Entity<TId>, IAuditableEntity
{
    public uint Version { get; private set; }
    public string Author { get; private set; } = string.Empty;
    public DateTime CreatedOnUtc { get; private set; }
    public string Editor { get; private set; } = string.Empty;
    public DateTime LastModifiedOnUtc { get; private set; }
}


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public sealed class NotAuditableAttribute : Attribute;
