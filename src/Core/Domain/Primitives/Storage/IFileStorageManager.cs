namespace eCommerceWeb.Domain.Primitives.Storage;

public interface IFileStorageManger
{
    Task ClearAsync(CancellationToken cancellationToken= default);
    Task DeleteAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default);
    Task<byte[]> GetByteArrayAsync(IFileEntry fileEntry, CancellationToken cancellationToken= default);
    Task<Stream> GetStreamAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default);
    Task<string> GetUriAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default);
    Task UploadAsync(IFileEntry fileEntry, Stream stream, CancellationToken cancellationToken = default);
    Task ArchiveAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default);
    Task UnArchiveAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default);
}
