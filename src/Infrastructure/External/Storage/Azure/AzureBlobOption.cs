using System.ComponentModel.DataAnnotations;

namespace eCommerceWeb.External.Storage.Azure;

public sealed class AzureBlobOption
{
    [Required]
    public string ConnectionString { get; set; } = string.Empty;
    [Required]
    public string ContainerName { get; set; } = string.Empty;
}
