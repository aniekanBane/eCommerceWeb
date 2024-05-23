using System.ComponentModel.DataAnnotations;

namespace eCommerceWeb.External.Storage.Local;

public sealed class LocalOption
{
    [Required]
    public string Path { get; set; } = string.Empty;
}
