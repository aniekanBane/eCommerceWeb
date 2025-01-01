using eCommerceWeb.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Constants;

namespace eCommerceWeb.Persistence.Converters;

internal sealed class PhoneNumberConverter() : ValueConverter<PhoneNumber, string>
(
    number => number.Number,
    v => new PhoneNumber(v),
    new(size: DomainModelConstants.PHONE_NUMBER_MAX_LENGTH)
);
