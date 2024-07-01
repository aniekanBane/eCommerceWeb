using eCommerceWeb.Domain.Primitives.Entities;

namespace eCommerceWeb.Domain.Entities;

public abstract class Tag : Entity<int>
{
    public string Name { get; private set; }
    public string NormalisedName { get; private set; }

    protected Tag(string name) : this()
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Guard.Against.StringTooLong(name, DomainModelConstants.TAG_NAME_MAX_LENGTH, nameof(name));
        
        Name = name;
    }

    #pragma warning disable CS8618
    protected Tag() { }
}
