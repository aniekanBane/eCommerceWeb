using Ardalis.SmartEnum;

namespace eCommerceWeb.Domain.Entities.CatalogAggregate;

public sealed record class GiftCardType
{
    private GiftCardTypeEnum _type = null!;
    public string Value 
    { 
        get => _type.Name; 
        private init => _type = Parse(value);
    }

    public GiftCardType(string value)
    {
        Value = value;
    }

    public static implicit operator string(GiftCardType value) => value.Value;

    public static GiftCardType Of(string value) => new(value);
    public static List<string> ListNames() => GiftCardTypeEnum.List.Select(e => e.Name).ToList();

    public static GiftCardType Virtual() => new(GiftCardTypeEnum.Virtual.Name);
    public static GiftCardType Physical() => new(GiftCardTypeEnum.Physical.Name);

    private static GiftCardTypeEnum Parse(string? value)
    {
        Guard.Against.NullOrWhiteSpace(value, nameof(value));
        var success = GiftCardTypeEnum.TryFromName(value, true, out var result);
        Guard.Against.Expression(x => !x, success, ErrorMessages.Product.InvalidGiftCardType);
        return result;
    }

    private GiftCardType() {} // EF Core

    private abstract class GiftCardTypeEnum(string name, ushort value) 
        : SmartEnum<GiftCardTypeEnum, ushort>(name, value)
    {
        public static readonly GiftCardTypeEnum Virtual = new VirtualType();
        public static readonly GiftCardTypeEnum Physical = new PhysicalType();

        private sealed class VirtualType() : GiftCardTypeEnum("Virtual", 1);
        public sealed class PhysicalType() : GiftCardTypeEnum("Physical", 2);
    }
}
