using eCommerceWeb.Domain.Primitives.Events;

namespace eCommerceWeb.Domain.Primitives.Entities;

public abstract class AuditableEntityWithDomainEvent<TId> : AuditableEntity<TId>, IHasDomainEvent
{
    private readonly List<DomainEvent> _domainEvents = [];

    [NotMapped]
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();
    public void RaiseDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void RemoveDomainEvent(DomainEvent domainEvent) => _domainEvents.Remove(domainEvent);
}
