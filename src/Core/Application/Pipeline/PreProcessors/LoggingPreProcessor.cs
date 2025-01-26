using eCommerceWeb.Domain.Primitives.Logging;
using MediatR.Pipeline;

namespace eCommerceWeb.Application.Pipeline.PreProcessors;

internal sealed class LoggingPreProcessor<TRequest>(IAppLogger<TRequest> logger)
    : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        logger.LogTrace("Processing Request {RequestName} -> {@Request}", requestName, request); // TODO: add user

        return Task.CompletedTask;
    }
}
