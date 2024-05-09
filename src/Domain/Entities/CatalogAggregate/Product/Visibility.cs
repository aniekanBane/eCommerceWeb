using Ardalis.SmartEnum;

namespace eCommerceWeb.Domain.Entities.CatalogAggregate;

public sealed record class Visibility
{
    private VisibilityEnum _visibility = null!;
    public string Value 
    { 
        get => _visibility.Name; 
        private set => _visibility = Parse(value); 
    }

    public Visibility(string value)
    {
        Value = value;
    }

    public static implicit operator string(Visibility value) => value.Value;

    public static Visibility Of(string value) => new(value);
    public static List<string> ListNames() => VisibilityEnum.List.Select(e => e.Name).ToList();

    public static Visibility Hidden() => new(VisibilityEnum.Hidden.Name);
    public static Visibility Public() => new(VisibilityEnum.Public.Name);
    public static Visibility Scheduled() => new(VisibilityEnum.Scheduled.Name);

    private static VisibilityEnum Parse(string value)
    {
        Guard.Against.NullOrWhiteSpace(value, nameof(value));
        var success = VisibilityEnum.TryFromName(value, true, out var result);
        Guard.Against.Expression(x => !x, success, ErrorMessages.Product.InvalidVisibility);
        return result;
    }

    private Visibility() {} // EF Core

    private abstract class VisibilityEnum(string name, ushort value) 
        : SmartEnum<VisibilityEnum, ushort>(name, value)
    {
        public static readonly VisibilityEnum Hidden = new HiddenType();
        public static readonly VisibilityEnum Public = new PublicType();
        public static readonly VisibilityEnum Scheduled = new ScheduledType();

        private sealed class HiddenType() : VisibilityEnum("Hidden", 1);
        private sealed class PublicType() : VisibilityEnum("Public", 2);
        private sealed class ScheduledType() : VisibilityEnum("Scheduled", 3);
    }
}
