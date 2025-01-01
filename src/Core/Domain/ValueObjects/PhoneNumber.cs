namespace eCommerceWeb.Domain.ValueObjects;

public sealed record class PhoneNumber
{
    public string Number { get; }

    public PhoneNumber(string number)
    {
        Guard.Against.NullOrWhiteSpace(number, nameof(number));
        Guard.Against.InvalidFormat(
            number, 
            nameof(number), 
            DomainModelConstants.PHONE_NUMBER_REGEX,
            ErrorMessages.ValueObjects.INVALID_PHONE_NUMBER
        );
        Number = NormalizeNumber(number);
    }

    #pragma warning disable CS8618
    private PhoneNumber() { } // EF Core
    #pragma warning restore CS8618

    public static PhoneNumber Of(string value) => new(value);
    public static explicit operator PhoneNumber(string value) => new(value);
    public static implicit operator string(PhoneNumber number) => number.Number;

    private static string NormalizeNumber(string number) 
        => number.TrimStart('+').Replace(" ", "").Replace("-", "");
}
