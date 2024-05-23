using eCommerceWeb.Application.Abstractions.Database;

namespace eCommerceweb.PublicApi.Extensions;

internal static class DatabaseExtensions
{
    public static async Task InitializeDatabase(this IApplicationBuilder builder)
    {
        using var scope = builder.ApplicationServices.CreateScope();
        var initializers = scope.ServiceProvider.GetServices<IDatabaseInitializer>().ToList();
        
        foreach (var initializer in initializers)
        {
            await initializer.InitializeAsync(); // apply migration
            await initializer.SeedAsync(); // seed
        }
    }
}
