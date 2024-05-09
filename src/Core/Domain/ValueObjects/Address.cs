namespace eCommerceWeb.Domain.ValueObjects;

public sealed record class Address
{
    public string Line1 { get; init; }
    public string? Line2 { get; init; }
    public string City { get; init; }
    public string StateProvince { get; init; }
    public string? ZipCode { get;  init; }
    public string Country { get; init; }

    public Address(string line1, string? line2, string city, string stateProvince, string? zipCode, string country)
    {
        Guard.Against.NullOrWhiteSpace(line1, nameof(line1));
        Guard.Against.StringTooLong(line1, DomainModelConstants.ADDRESS_LINE_MAX_LENGTH, nameof(line1));
        Guard.Against.NullOrWhiteSpace(city, nameof(city));
        Guard.Against.StringTooLong(city, DomainModelConstants.CITY_MAX_LENGTH, nameof(city));
        Guard.Against.NullOrWhiteSpace(stateProvince, nameof(stateProvince));
        Guard.Against.NullOrWhiteSpace(country, nameof(country));
        Guard.Against.StringTooLong(country, DomainModelConstants.COUNTRY_MAX_LENGTH, nameof(country));

        if (line2 is not null)
            Guard.Against.StringTooLong(line2, DomainModelConstants.ADDRESS_LINE_MAX_LENGTH, nameof(line2));
        if (zipCode is not null)
            Guard.Against.StringTooLong(zipCode, DomainModelConstants.ZIPCODE_MAX_LENGTH, nameof(zipCode));

        Line1 = line1;
        Line2 = line2;
        City = city;
        ZipCode = zipCode;
        Country = country;
        StateProvince = stateProvince;
    }

    #pragma warning disable CS8618
    private Address() { } //EF Core
}
