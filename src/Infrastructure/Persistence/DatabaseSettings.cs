using Microsoft.Extensions.Configuration;

namespace eCommerceWeb.Persistence;

public sealed class DatabaseSettings
{
    public const string CONFIG_SECTION = nameof(DatabaseSettings);

    public const string StoreDb = nameof(StoreDb);

    public required string ConnectionString { get; set; }
}

internal static class DatabaseSettingsExtensions
{
    public static IConfigurationSection GetDatabaseSettings(this IConfiguration config, string key) 
        => config.GetSection($"{DatabaseSettings.CONFIG_SECTION}:{key}");
    
    public static string GetDatabaseSettings(string key) 
        => $"{DatabaseSettings.CONFIG_SECTION}:{key}";
}
