using eCommerceWeb.Domain.Primitives.SysTime;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerceWeb.External.SysDateTime;

public static class DateTimeProviderExtensions
{
    public static IServiceCollection AddDateTimeProvider(this IServiceCollection services)
    {
        return services.AddTransient<IDateTimeProvider, DateTimeProvider>();
    }
}
