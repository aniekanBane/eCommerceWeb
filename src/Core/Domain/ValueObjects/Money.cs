namespace eCommerceWeb.Domain.ValueObjects;

// source: https://github.com/asc-lab/better-code-with-ddd/blob/ef_core/LoanApplication.TacticalDdd/LoanApplication.TacticalDdd/DomainModel/MonetaryAmount.cs

public sealed record class Money
{
    public static readonly Money Zero = new(0M);

    public decimal Amount { get; }

    public Money(decimal amount) => Amount = decimal.Round(amount, 2, MidpointRounding.ToEven);

    public static Money Of(decimal amount) => new(amount);

    public Money Add(Money otherMoney) => new(Amount + otherMoney.Amount);
    public Money Add(decimal amount) => Add(new Money(amount));
    public Money DivideBy(Money money) => new(Amount / money.Amount);
    public Money DivideBy(decimal amount) => DivideBy(new Money(amount));
    public Money MultiplyBy(Money money) => new(Amount * money.Amount);
    public Money MultiplyBy(decimal amount) => MultiplyBy(new Money(amount));
    public Money MultiplyByPercent(Percent percent) => new(Amount * percent.Value / 100M);
    public Money MultiplyByPercent(decimal percent) => MultiplyByPercent(new Percent(percent));
    public Money Subtract(Money otherMoney) => new(Amount - otherMoney.Amount);
    public Money Subtract(decimal amount) => Subtract(new Money(amount));

    public static Money operator +(Money first, Money second) => first.Add(second);
    public static Money operator -(Money first, Money second) => first.Subtract(second);
    public static Money operator *(Money first, Money second) => first.MultiplyBy(second);
    public static Money operator *(Money money, Percent percent) => money.MultiplyByPercent(percent);
    public static Money operator /(Money first, Money second) => first.DivideBy(second);

    public static bool operator <(Money left, Money right) => left.CompareTo(right) < 0;
    public static bool operator >(Money left, Money right) => left.CompareTo(right) > 0;
    public static bool operator <=(Money left, Money right) => left.CompareTo(right) <= 0;
    public static bool operator >=(Money left, Money right) => left.CompareTo(right) >= 0;

    private int CompareTo(Money other) => Amount.CompareTo(other.Amount);
}
