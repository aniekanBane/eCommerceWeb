using eCommerceWeb.Domain.Primitives.Entities;

namespace eCommerceWeb.Domain.Entities.Directory;

public sealed class Country(string name, string cca2, string cca3, int ccn3) 
    : AuditableEntity<int>
{
    /// <summary>
    /// Name of Country
    /// </summary>
    public string Name { get; private set; } = name;
    public string NormalisedName
    {
        get => Name.ToUpper();
        private set { }
    }

    /// <summary>
    /// Two letter Iso code.
    /// </summary>
    public string Cca2 { get; private set; } = cca2;

    /// <summary>
    /// Three letter Iso code.
    /// </summary>
    public string Cca3 { get; private set; } = cca3;

    /// <summary>
    /// Three digit numeric Iso code.
    /// </summary>
    public int Ccn3 { get; private set; } = ccn3;

    public bool ShippingEnabled { get; private set; }

    public Country ToggleShipping(bool value)
    {
        ShippingEnabled = value;
        return this;
    }
}
