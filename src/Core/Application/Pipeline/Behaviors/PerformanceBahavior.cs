using System.Diagnostics;
using eCommerceWeb.Domain.Primitives.Logging;
using MediatR;

namespace eCommerceWeb.Application.Pipeline.Behaviors;

public class PerformanceBehavior<TRequest, TResponse>(IAppLogger<PerformanceBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        Interlocked.Increment(ref RequestCounter.ExecutionCount);
        var startTime = RequestCounter.ExecutionCount > 2 ? Stopwatch.GetTimestamp() : 0;

        var response = await next();

        var delta = startTime > 0 ? Stopwatch.GetElapsedTime(startTime) : TimeSpan.Zero;
        if (delta.Milliseconds > 100)
        {
            logger.LogWarning(
                "Long running request: {RequestName} {EllapsedSeconds:F2}ms\n{@Request}", 
                typeof(TRequest).Name, delta.Milliseconds, request
            );
        }

        return response;
    }
}

public static class RequestCounter
{
    internal static int ExecutionCount;
}
