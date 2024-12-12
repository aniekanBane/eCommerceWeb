namespace eCommerceWeb.Domain.ValueObjects;

public sealed record class EmailAddress
{
    public string Value { get; }

    public EmailAddress(string value)
    {
        Guard.Against.NullOrWhiteSpace(value, nameof(value));
        Guard.Against.InvalidFormat(value, nameof(value), DomainModelConstants.EMAIL_REGEX);
        Value = value;
    }

    #pragma warning disable CS8618
    private EmailAddress() { } // EF Core
    #pragma warning restore CS8618

    public static EmailAddress Of(string value) => new(value);
    public static explicit operator EmailAddress(string value) => new(value);
    public static implicit operator string(EmailAddress emailAddress) => emailAddress.Value;
}
