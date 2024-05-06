using eCommerceWeb.Domain.Primitives.Entities;

namespace eCommerceWeb.Domain.Primitives.Events;

public class CreatedEvent<TEntity>(TEntity entity) : DomainEvent where TEntity : IEntity
{
    public TEntity Entity { get; } = entity;
}
