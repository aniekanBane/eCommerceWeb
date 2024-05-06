namespace eCommerceWeb.Domain.Primitives.Storage;

public interface IFileStorageProvider
{
    Task ClearAsync(CancellationToken cancellationToken= default);
    Task CreateAsync(IFileEntry fileEntry, Stream stream, CancellationToken cancellationToken = default);
    Task DeleteAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default);
    Task<byte[]> ReadAsByteArrayAsync(IFileEntry fileEntry, CancellationToken cancellationToken= default);
    Task<string> ReadAsStringAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default);
}
