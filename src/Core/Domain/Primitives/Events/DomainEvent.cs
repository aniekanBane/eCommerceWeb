namespace eCommerceWeb.Domain.Primitives.Events;

public abstract class DomainEvent
{
    public bool IsPublished { get; set; }
    public DateTime OccuredAt { get; protected set; } = DateTime.UtcNow;
}
