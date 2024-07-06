using FluentValidation;
using MediatR.Pipeline;

namespace eCommerceWeb.Application.Pipeline.PreProcessors;

internal class ValidationPreProcessor<TRequest>(
    IEnumerable<IValidator<TRequest>> validators
) : IRequestPreProcessor<TRequest>
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

        if (falures.Count != 0) throw new ValidationException(falures);
    }
}
