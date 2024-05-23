using eCommerceWeb.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace eCommerceWeb.Persistence.Converters;

internal sealed class PercentConverter() : ValueConverter<Percent, decimal>
(
    v => v.Value,
    v => new Percent(v),
    new ConverterMappingHints(precision: 5, scale: 2)
);
