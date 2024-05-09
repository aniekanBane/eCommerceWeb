namespace eCommerceWeb.Domain.ValueObjects;

public sealed record class Name
{
    public string Firstname { get; init; }
    public string Lastname { get; init; }
    public string Fullname 
    { 
        get => string.Join(' ', Firstname, Lastname); 
        private init {}
    }

    public Name(string firstname, string lastname)
    {
        Guard.Against.StringTooLong(firstname, DomainModelConstants.NAME_MAX_LENGTH, nameof(firstname));
        Guard.Against.StringTooLong(lastname, DomainModelConstants.NAME_MAX_LENGTH, nameof(lastname));

        Firstname = firstname;
        Lastname = lastname;
    }

    #pragma warning disable CS8618
    private Name() {} // EF Core
}
