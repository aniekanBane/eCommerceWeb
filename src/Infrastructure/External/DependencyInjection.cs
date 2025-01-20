using eCommerceWeb.Domain.Primitives.Csv;
using eCommerceWeb.Domain.Primitives.Excel;
using eCommerceWeb.Domain.Primitives.Logging;
using eCommerceWeb.External.Csv;
using eCommerceWeb.External.Excel;
using eCommerceWeb.External.Logging;
using eCommerceWeb.External.Storage;
using eCommerceWeb.External.SysDateTime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerceWeb.External;

public static class DependencyInjection
{   
    public static IServiceCollection AddExternalServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));
        services.AddDateTimeProvider();
        return services;
    }

    public static IServiceCollection AddImportExportServices(this IServiceCollection services)
    {
        return services
            .AddScoped<ICsvReader, CsvService>()
            .AddScoped<ICsvWriter, CsvService>()
            .AddScoped<IExcelReader, ClosedXMLService>()
            .AddScoped<IExcelWriter, ClosedXMLService>();
    }

    public static IServiceCollection AddStorageManger(this IServiceCollection services, IConfiguration configuration)
    {
        StorageOptions? storageOptions = configuration.GetRequiredSection(StorageOptions.CONFIG_SECTION)
            .Get<StorageOptions>() ?? throw new InvalidOperationException();
            
        if (storageOptions.IsAzure)
        {
            services.AddAzureBlobStorageManager(storageOptions.Azure!);
        }
        else if (storageOptions.IsLocal)
        {
            services.AddLocalStorageManager(storageOptions.Local!);
        }

        return services;
    }
}
