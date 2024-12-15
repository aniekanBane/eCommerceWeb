using Ardalis.SmartEnum;

namespace eCommerceWeb.Domain.Entities.MarketingAggregate;

public sealed record SubcriberStatus
{
    private SubcriberStatusEnum _status = null!;
    public string Value
    {
        get => _status.Name;
        private init { _status = Parse(value); }
    }

    public SubcriberStatus(string status)
    {
        Value = status;
    }

    private SubcriberStatus() { } // EF Core

    public static implicit operator string(SubcriberStatus status) => status.Value;

    public static SubcriberStatus Of(string value) => new(value);
    public static List<string> ListNames() => SubcriberStatusEnum.List.Select(x => x.Name).ToList();

    public static SubcriberStatus Cleaned() => new(SubcriberStatusEnum.Cleaned.Name);
    public static SubcriberStatus Subcribed() => new(SubcriberStatusEnum.Subcribed.Name);
    public static SubcriberStatus UnSubcribed() => new(SubcriberStatusEnum.UnSubcribed.Name);

    private static SubcriberStatusEnum Parse(string value)
    {
        Guard.Against.NullOrWhiteSpace(value, nameof(value));
        var success = SubcriberStatusEnum.TryFromName(value, true, out var result);
        Guard.Against.Expression(x => !x, success, ErrorMessages.MailingList.InvalidSubcribtionType);
        return result;
    }

    private abstract class SubcriberStatusEnum(string name, ushort value) 
        : SmartEnum<SubcriberStatusEnum, ushort>(name, value)
    {
        public static readonly SubcriberStatusEnum Cleaned = new CleanedType();
        public static readonly SubcriberStatusEnum Subcribed = new SubcribedType();
        public static readonly SubcriberStatusEnum UnSubcribed = new UnSubcribedType();

        private sealed class CleanedType() : SubcriberStatusEnum("Cleaned", 0);
        private sealed class SubcribedType() : SubcriberStatusEnum("Subcribed", 1);
        private sealed class UnSubcribedType() : SubcriberStatusEnum("UnSubcribed", 2);
    }
}
