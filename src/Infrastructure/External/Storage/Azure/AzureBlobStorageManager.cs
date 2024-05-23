using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using eCommerceWeb.Domain.Primitives.Storage;

namespace eCommerceWeb.External.Storage.Azure;

internal sealed class AzureBlobStorageManager(BlobContainerClient blobContainerClient) : IFileStorageManger
{
    public async Task ArchiveAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        var blob = blobContainerClient.GetBlobClient(fileEntry.FileLocation);
        await blob.SetAccessTierAsync(AccessTier.Cool, cancellationToken: cancellationToken);
    }

    public async Task ClearAsync(CancellationToken cancellationToken = default)
    {
        var blobs = blobContainerClient.GetBlobsAsync(cancellationToken: cancellationToken);
        await foreach (var blob in blobs)
        {
            await blobContainerClient.DeleteBlobIfExistsAsync(blob.Name, cancellationToken: cancellationToken);
        }
    }

    public async Task DeleteAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        var blob = blobContainerClient.GetBlobClient(fileEntry.FileLocation);
        await blob.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    public async Task<byte[]> GetByteArrayAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        var blob = blobContainerClient.GetBlobClient(fileEntry.FileLocation);
        var response = await blob.DownloadContentAsync(cancellationToken);

        return response.Value.Content.ToArray();
    }

    public async Task<Stream> GetStreamAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        var array = await GetByteArrayAsync(fileEntry, cancellationToken);
        return new MemoryStream(array);
    }

    public async Task<string> GetUriAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        var blob = blobContainerClient.GetBlobClient(fileEntry.FileLocation);
        return await blob.ExistsAsync(cancellationToken) 
            ? blob.Uri.AbsoluteUri 
            : string.Empty;
    }

    public async Task UnArchiveAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        var blob = blobContainerClient.GetBlobClient(fileEntry.FileLocation);
        await blob.SetAccessTierAsync(AccessTier.Hot, cancellationToken: cancellationToken);
    }

    public async Task UploadAsync(IFileEntry fileEntry, Stream stream, CancellationToken cancellationToken = default)
    {
        stream.Position = 0;

        await blobContainerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        var blob = blobContainerClient.GetBlobClient(fileEntry.FileLocation);
        await blob.UploadAsync(
            stream,
            new BlobHttpHeaders { ContentType = fileEntry.ContentType },
            cancellationToken: cancellationToken);
    }
}
