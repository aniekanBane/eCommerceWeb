using eCommerceWeb.External.Storage.Azure;
using eCommerceWeb.External.Storage.Local;

namespace eCommerceWeb.External.Storage;

public sealed class StorageOptions
{
    public const string CONFIG_SECTION = nameof(StorageOptions);
    
    public required string Provider { get; set; }

    public AzureBlobOption? Azure { get; set; }
    public LocalOption? Local { get; set; }

    public bool IsLocal => Provider == "Local";
    public bool IsAzure => Provider == "Azure";
}
