using eCommerceWeb.Domain.ValueObjects;

namespace eCommerceWeb.Application.Abstractions.Mappings;

public static class MoneyMapper
{
    public static decimal MoneyToDecimal(Money money) => money.Amount;
}

public static class PercentMapper
{
    public static decimal PercentToDecimal(Percent percent) => percent.Value;
}
