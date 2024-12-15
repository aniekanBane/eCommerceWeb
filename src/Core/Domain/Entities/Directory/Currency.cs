using eCommerceWeb.Domain.Primitives.Entities;

namespace eCommerceWeb.Domain.Entities.Directory;

public sealed class Currency : AuditableEntity<int>
{
    public Currency(string name, string code, string symbol)
    {
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Code = Guard.Against.NullOrWhiteSpace(code, nameof(code));
        Symbol = Guard.Against.NullOrWhiteSpace(symbol, nameof(symbol));
    }

    #pragma warning disable CS8618
    private Currency() { } // EF Core
    #pragma warning restore CS8618

    public string Name { get; private set; }
    public string NormalisedName
    {
        get => Name.ToUpper();
        private set { }
    }

    public string Code { get; private set; }

    public string Symbol { get; private set; }

    public bool HasUniqueSymbol { get; private set; }
}
