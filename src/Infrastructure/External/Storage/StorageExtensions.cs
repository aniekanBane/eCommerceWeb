using Azure.Storage.Blobs;
using eCommerceWeb.Domain.Primitives.Storage;
using eCommerceWeb.External.Storage.Azure;
using eCommerceWeb.External.Storage.Local;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace eCommerceWeb.External.Storage;

public static class StorageExtensions
{
    internal static void AddAzureBlobStorageManager(this IServiceCollection services, AzureBlobOption option)
    {
        services.AddSingleton(_ => new BlobContainerClient(option.ConnectionString, option.ContainerName));
        services.AddSingleton<IFileStorageManger, AzureBlobStorageManager>();
    }

    internal static void AddLocalStorageManager(this IServiceCollection services, LocalOption option)
    {
        services.AddSingleton<IFileStorageManger>(sp 
            => new LocalStorageManager(option, sp.GetRequiredService<ILogger<LocalStorageManager>>())
        );
    }
}
