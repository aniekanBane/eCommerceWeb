using eCommerceWeb.Domain.Primitives.Entities;

namespace eCommerceWeb.Domain.Entities.Directory;

public sealed class Currency(string name, string code, string symbol) 
    : AuditableEntity<int>
{
    public string Name { get; private set; } = name;
    public string NormalisedName
    {
        get => Name.ToUpper();
        private set { }
    }

    public string Code { get; private set; } = code;

    public string Symbol { get; private set; } = symbol;

    public bool HasUniqueSymbol { get; private set; }
}
