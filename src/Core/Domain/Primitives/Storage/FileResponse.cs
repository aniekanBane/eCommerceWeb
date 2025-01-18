namespace eCommerceWeb.Domain.Primitives.Storage;

public sealed record class FileResponse(Stream Stream, string ContentType, string FileId);
