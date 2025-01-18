using eCommerceWeb.Application.Abstractions.Database;
using eCommerceWeb.Domain.Primitives.Repositories;
using eCommerceWeb.Persistence.Interceptors;
using eCommerceWeb.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace eCommerceWeb.Persistence;

public static class DependencyInjection
{
    private const string MIGRATION_ASSEMBLY = "Migrator";

    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddSingleton<AuditableEntitySaveInterceptor>();

        services.AddOptions<DatabaseSettings>(DatabaseSettings.StoreDb)
            .BindConfiguration($"{DatabaseSettings.CONFIG_SECTION}:{DatabaseSettings.StoreDb}")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddDbContext<StoreDbContext>((sp, opts) => 
        {
            var connection = sp.GetRequiredService<IOptionsSnapshot<DatabaseSettings>>()
                .Get(DatabaseSettings.StoreDb).ConnectionString;
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
