using eCommerceWeb.Domain.Primitives.Entities;

namespace eCommerceWeb.Domain.Entities.Directory;

public abstract class DirectoryBase : Entity<int>
{
    public DirectoryBase(string name)
    {
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
    }

    #pragma warning disable CS8618
    protected DirectoryBase() { } // EF Core
    #pragma warning restore CS8618

    public string Name { get; private set; }
    
    public string NormalizedName 
    { 
        get => Name.ToUpper(); 
        private set { }
    } 
}
