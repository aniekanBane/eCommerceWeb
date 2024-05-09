using eCommerceWeb.Domain.Primitives.Events;

namespace eCommerceWeb.Domain.Primitives.Entities;

public interface IHasDomainEvent
{
    IReadOnlyCollection<DomainEvent> DomainEvents { get; }

    void ClearDomainEvents();
}
