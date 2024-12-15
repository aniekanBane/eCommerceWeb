using eCommerceWeb.Domain.Primitives.Entities;

namespace eCommerceWeb.Domain.Entities.Directory;

public sealed class Country : AuditableEntity<int>
{
    public Country(string name, string cca2, string cca3, string ccn3)
    {
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Cca2 = Guard.Against.InvalidStringLength(2, cca2, nameof(cca2));
        Cca3 = Guard.Against.InvalidStringLength(3, cca3, nameof(cca3));
        Ccn3 = Guard.Against.InvalidStringLength(3, ccn3, nameof(ccn3));
    }

    #pragma warning disable CS8618
    private Country() { } // EF Core
    #pragma warning restore CS8618

    /// <summary>
    /// Name of Country
    /// </summary>
    public string Name { get; private set; }
    public string NormalisedName
    {
        get => Name.ToUpper();
        private set { }
    }

    /// <summary>
    /// Two letter Iso code.
    /// </summary>
    public string Cca2 { get; private set; }

    /// <summary>
    /// Three letter Iso code.
    /// </summary>
    public string Cca3 { get; private set; }

    /// <summary>
    /// Three digit numeric Iso code.
    /// </summary>
    public string Ccn3 { get; private set; }

    public bool ShippingEnabled { get; private set; }

    public Country ToggleShipping(bool value)
    {
        ShippingEnabled = value;
        return this;
    }
}
