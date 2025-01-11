namespace eCommerceWeb.Domain.Entities.Directory;

public sealed class Country : DirectoryBase
{
    public Country(string name, string cca2, string cca3, string ccn3) : base(name)
    {
        Cca2 = Guard.Against.InvalidStringLength(2, cca2, nameof(cca2));
        Cca3 = Guard.Against.InvalidStringLength(3, cca3, nameof(cca3));
        Ccn3 = Guard.Against.InvalidStringLength(3, ccn3, nameof(ccn3));
    }

    #pragma warning disable CS8618
    private Country() { } // EF Core
    #pragma warning restore CS8618

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

    private readonly List<StateProvince> _stateProvinces = [];
    public IReadOnlyCollection<StateProvince> StateProvinces => _stateProvinces.AsReadOnly();
}
