using System.Reflection;
using eCommerceWeb.Domain.Primitives.SysTime;
using eCommerceWeb.External.SysTimeProvider;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerceWeb.External;

public static class DependencyInjection
{
    public static readonly Assembly AssemblyReference = typeof(DependencyInjection).Assembly;
    
    public static IServiceCollection AddExternalServices(this IServiceCollection services)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        return services;
    }
}
