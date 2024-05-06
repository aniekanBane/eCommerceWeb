using eCommerceWeb.Domain.Primitives.Entities;

namespace eCommerceWeb.Domain.Entities;

public abstract class Tag : Entity<int>
{
    public string Name { get; private set; }
    public string NormalisedName 
    {
        get => Name.ToUpper();
        private set {}
    }

    protected Tag(string name) : this()
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Name = name;
    }

    #pragma warning disable CS8618
    protected Tag() { }
}
