namespace eCommerceWeb.Application.Models;

public class UploadRequest
{
    public required string FileName { get; set; }
    public required string ContentType { get; set; }
    public string? FileExtension { get; set; }
    public required Stream Content { get; set; }
}
