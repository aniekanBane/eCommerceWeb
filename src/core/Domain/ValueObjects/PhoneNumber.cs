namespace eCommerceWeb.Domain.ValueObjects;

public sealed record class PhoneNumber
{
    public string Number { get; } = default!;

    public PhoneNumber(string number)
    {
        Guard.Against.NullOrWhiteSpace(number, nameof(number));
        Guard.Against.InvalidFormat(number, nameof(number), DomainModelConstants.PHONE_NUMBER_REGEX);
        Number = number.TrimStart('+');
    }

    private PhoneNumber() {}

    public static PhoneNumber Of(string value) => new(value);
    public static explicit operator PhoneNumber(string value) => new(value);
    public static implicit operator string(PhoneNumber number) => number.Number;
}
