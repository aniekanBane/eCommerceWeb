namespace eCommerceWeb.Domain.Primitives.Entities;

public abstract class Entity<TId> : IEntity<TId>, IEquatable<Entity<TId>>
{
    protected Entity() { }

    protected Entity(TId id) => Id = id;

    [Column(Order = 0)]
    public TId Id { get; protected init; } = default!;

    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
    {
        return !(left == right);
    }

    public bool Equals(Entity<TId>? other)
    {
        if (other is null || GetType() != other.GetType())
            return false;

        return EqualityComparer<TId>.Default.Equals(other.Id, Id);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || Equals(obj as Entity<TId>);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<TId>.Default.GetHashCode(Id!);
    }
}
