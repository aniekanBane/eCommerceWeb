namespace eCommerceWeb.Domain.Entities.Directory;

public sealed class StateProvince : DirectoryBase
{
    public StateProvince(string name) : base(name) { }

    private StateProvince() { } // EF Core

    public int CountryId { get; private set; }
}
