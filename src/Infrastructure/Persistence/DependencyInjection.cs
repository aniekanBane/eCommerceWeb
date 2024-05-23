using System.Reflection;
using eCommerceWeb.Application.Abstractions.Database;
using eCommerceWeb.Domain.Primitives.Repositories;
using eCommerceWeb.Persistence.Interceptors;
using eCommerceWeb.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerceWeb.Persistence;

public static class DependencyInjection
{
    private const string MIGRATION_ASSEMBLY = "Migrator";
    public static readonly Assembly AssemblyReference = typeof(DependencyInjection).Assembly;

    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<AuditableEntitySaveInterceptor>();

        services.AddDbContext<StoreDbContext>((sp, opts) => 
        {
            var connection = configuration.GetConnectionString("StoreDbConnection");
            opts.UseNpgsql(connection, npgopts =>
            {
                npgopts.MigrationsAssembly(MIGRATION_ASSEMBLY);
            });
            opts.UseSnakeCaseNamingConvention();
            opts.AddInterceptors(sp.GetRequiredService<AuditableEntitySaveInterceptor>());
        }).AddTransient<IDatabaseInitializer, StoreDbContextInitializer>();

        services.AddRepositories();

        return services;
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork>(s => s.GetRequiredService<StoreDbContext>());
        services.AddScoped(typeof(IReadRepository<>), typeof(StoreRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(StoreRepository<>));
    }
}
