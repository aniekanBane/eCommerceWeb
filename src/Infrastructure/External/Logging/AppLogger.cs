using eCommerceWeb.Domain.Primitives.Logging;
using Microsoft.Extensions.Logging;

namespace eCommerceWeb.External.Logging;

internal sealed class AppLogger<T>(ILoggerFactory loggerFactory) : IAppLogger<T>
{
    private readonly ILogger<T> logger = loggerFactory.CreateLogger<T>();

    public void LogError(Exception ex, string message, params object[] args)
    {
        logger.LogError(ex, message, args);
    }

    public void LogInformation(string message, params object[] args)
    {
        logger.LogInformation(message, args);
    }

    public void LogTrace(string message, params object[] args)
    {
        logger.LogTrace(message, args);
    }

    public void LogWarning(string message, params object[] args)
    {
        logger.LogWarning(message, args);
    }
}
