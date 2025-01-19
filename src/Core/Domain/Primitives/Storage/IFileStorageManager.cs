namespace eCommerceWeb.Domain.Primitives.Storage;

public interface IFileStorageManger
{
    Task ArchiveAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default);
    Task ClearAsync(CancellationToken cancellationToken = default);
    Task DeleteAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default);
    Task<FileResponse> DownloadAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default);
    Task<FileResponse> UploadAsync(IFileEntry fileEntry, Stream stream, CancellationToken cancellationToken = default);
    Task UnArchiveAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default);
}
