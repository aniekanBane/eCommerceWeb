using System.Reflection;
using eCommerceWeb.Domain.Primitives.Logging;
using eCommerceWeb.Domain.Primitives.SysTime;
using eCommerceWeb.External.Logging;
using eCommerceWeb.External.SysDateTime;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerceWeb.External;

public static class DependencyInjection
{
    public static readonly Assembly AssemblyReference = typeof(DependencyInjection).Assembly;
    
    public static IServiceCollection AddExternalServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        return services;
    }
}
