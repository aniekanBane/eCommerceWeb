using eCommerceWeb.Domain.Primitives.Logging;
using FluentValidation;
using MediatR.Pipeline;

namespace eCommerceWeb.Application.Pipeline.PreProcessors;

internal class ValidationPreProcessor<TRequest>(
    IEnumerable<IValidator<TRequest>> validators,
    IAppLogger<ValidationPreProcessor<TRequest>> logger) 
    : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        if (!validators.Any()) return;

        var context = new ValidationContext<TRequest>(request);

        var results = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var falures = results
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (falures.Count != 0) 
        {
            logger.LogWarning(
                "Validation errors - {RequestType} - Request: {@Request} - Errors: {@ValidationErrors}",
                typeof(TRequest).Name, request, falures
            );
            throw new ValidationException(falures);
        }
    }
}
