namespace eCommerceWeb.Domain.Primitives.Entities;

public interface IEntity<TId> : IEntity
{
    TId Id { get; }
}

public interface IEntity;
