namespace eCommerceWeb.Domain.ValueObjects;

// source: https://github.com/asc-lab/better-code-with-ddd/blob/ef_core/LoanApplication.TacticalDdd/LoanApplication.TacticalDdd/DomainModel/Percent.cs

public sealed record class Percent
{
    public static readonly Percent Zero = new(0M);
    public static readonly Percent Hundered = new(100M);

    public decimal Value { get; }

    public Percent(decimal value)
    {
        Guard.Against.Negative(value, nameof(value));
        Value = value;
    }

    private Percent() { } // EF Core

    public static Percent Of(decimal value) => new(value);

    public static bool operator >(Percent left, Percent right) => left.CompareTo(right)>0; 
    public static bool operator <(Percent left, Percent right) => left.CompareTo(right)<0; 
    public static bool operator >=(Percent left, Percent right) => left.CompareTo(right)>=0; 
    public static bool operator <=(Percent left, Percent right) => left.CompareTo(right)<=0;

    private int CompareTo(Percent other) => Value.CompareTo(other.Value);
}

public static class PercentExtensions
{
    public static Percent Percent(this int value) => new(value);
    public static Percent Percent(this decimal value) => new(value);
}
