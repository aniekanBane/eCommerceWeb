namespace eCommerceWeb.Domain.ValueObjects;

public sealed record class Name
{
    public string Firstname { get; }
    public string Lastname { get; }
    public string Fullname { get; }

    #pragma warning disable CS8618
    public Name(string firstname, string lastname)
    {
        Guard.Against.StringTooLong(firstname, DomainModelConstants.NAME_MAX_LENGTH, nameof(firstname));
        Guard.Against.StringTooLong(lastname, DomainModelConstants.NAME_MAX_LENGTH, nameof(lastname));

        Firstname = firstname;
        Lastname = lastname;
    }

    private Name() { } // EF Core
    #pragma warning restore CS8618
}
