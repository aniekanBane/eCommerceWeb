using eCommerceWeb.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace eCommerceWeb.Persistence.Converters;

internal sealed class MoneyConverter() : ValueConverter<Money, decimal>
(
    v => v.Amount,
    v => new Money(v),
    new ConverterMappingHints(precision: 10, scale: 2)
);
