namespace eCommerceWeb.Domain.ValueObjects;

public sealed record class Name
{
    public string Firstname { get; init; }
    public string Lastname { get; init; }
    public string Fullname { get; init; }

    #pragma warning disable CS8618

    public Name(string firstname, string lastname)
    {
        Guard.Against.StringTooLong(firstname, DomainModelConstants.NAME_MAX_LENGTH, nameof(firstname));
        Guard.Against.StringTooLong(lastname, DomainModelConstants.NAME_MAX_LENGTH, nameof(lastname));

        Firstname = firstname;
        Lastname = lastname;
    }

    private Name() { } // EF Core
}
