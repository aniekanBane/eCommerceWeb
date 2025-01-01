using Ardalis.SmartEnum;

namespace eCommerceWeb.Domain.Entities.CatalogAggregate;

public sealed record class Visibility
{
    private VisibilityEnum _visibility = null!;

    public Visibility(string value)
    {
        Value = value;
    }

    private Visibility() { } // EF Core

    public string Value 
    { 
        get => _visibility.Name; 
        private set => Guard.Against.InvalidInput(
            value, 
            nameof(value),
            x => VisibilityEnum.TryFromName(x, true, out _visibility),
            ErrorMessages.Product.InvalidVisibility
        );
    }

    public static implicit operator string(Visibility value) => value.Value;

    public static Visibility Of(string value) => new(value);
    public static List<string> ListNames() => [.. VisibilityEnum.List.Select(e => e.Name)];

    public static Visibility Hidden() => new(VisibilityEnum.Hidden.Name);
    public static Visibility Public() => new(VisibilityEnum.Public.Name);
    public static Visibility Scheduled() => new(VisibilityEnum.Scheduled.Name);

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
