using eCommerceWeb.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Constants;

namespace eCommerceWeb.Persistence.Converters;

internal sealed class EmailAddressConverter() : ValueConverter<EmailAddress, string>
(
    v => v.Value,
    v => new EmailAddress(v),
    new ConverterMappingHints(DomainModelConstants.EMAIL_MAX_LENGTH)
);

