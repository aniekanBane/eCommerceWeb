using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using eCommerceWeb.Domain.Primitives.Storage;
using Microsoft.Extensions.Logging;

namespace eCommerceWeb.External.Storage.Azure;

internal sealed class AzureBlobStorageManager(
    BlobContainerClient blobContainerClient,
    ILogger<AzureBlobStorageManager> logger) : IFileStorageManger
{
    public async Task ArchiveAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        var blob = blobContainerClient.GetBlobClient(fileEntry.FileLocation);
        await blob.SetAccessTierAsync(AccessTier.Cool, cancellationToken: cancellationToken);
        logger.LogInformation("Archived file: {File}", fileEntry.FileLocation);
    }

    public async Task ClearAsync(CancellationToken cancellationToken = default)
    {
        var blobs = blobContainerClient.GetBlobsAsync(cancellationToken: cancellationToken);
        logger.LogWarning("Clearing all files from storage container: {Conainer}", blobContainerClient.Name);
        await foreach (var blob in blobs)
        {
            await blobContainerClient.DeleteBlobIfExistsAsync(blob.Name, cancellationToken: cancellationToken);
        }
        logger.LogInformation("Cleared all files from storage container: {Container}", blobContainerClient.Name);
    }

    public async Task DeleteAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        var blob = blobContainerClient.GetBlobClient(fileEntry.FileLocation);
        var response = await blob.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        if (response is not null && response.Value)
        {
            logger.LogInformation("Deleted file: {File}", fileEntry.FileLocation);
        }
    }

    public async Task<FileResponse> DownloadAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        var fileId = fileEntry.FileLocation;
        var blob = blobContainerClient.GetBlobClient(fileId);
        var response = await blob.DownloadContentAsync(cancellationToken);
        logger.LogInformation("Successfully downloaded file: {File}", fileId);
        return new(response.Value.Content.ToStream(), response.Value.Details.ContentType, fileId);
    }

    public async Task UnArchiveAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        var blob = blobContainerClient.GetBlobClient(fileEntry.FileLocation);
        await blob.SetAccessTierAsync(AccessTier.Hot, cancellationToken: cancellationToken);
        logger.LogInformation("Unarchived file: {File}", fileEntry.FileLocation);
    }

    public async Task<FileResponse> UploadAsync(IFileEntry fileEntry, Stream stream, CancellationToken cancellationToken = default)
    {   
        await blobContainerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        string fileId = string.IsNullOrWhiteSpace(fileEntry.FileLocation) 
            ? fileEntry.Id.ToString("N")
            : fileEntry.FileLocation;

        var blob = blobContainerClient.GetBlobClient(fileEntry.FileLocation);
        await blob.UploadAsync(
            stream,
            new BlobHttpHeaders { ContentType = fileEntry.ContentType },
            cancellationToken: cancellationToken
        );

        logger.LogInformation("Uploaded file to storage container: {File}", fileEntry.FileName);

        return new(Stream.Null, fileEntry.ContentType, fileId);
    }
}
