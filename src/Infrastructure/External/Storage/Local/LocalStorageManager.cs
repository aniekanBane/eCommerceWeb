using eCommerceWeb.Domain.Primitives.Storage;
using Microsoft.Extensions.Logging;

namespace eCommerceWeb.External.Storage.Local;

internal sealed class LocalStorageManager(
    LocalOption option, 
    ILogger<LocalStorageManager> logger) : IFileStorageManger, IDisposable
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public Task ArchiveAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Archiving file: {FileName}", fileEntry.FileName);
        return Task.CompletedTask; // TODO: Implement local archiving
    }

    public async Task ClearAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _semaphore.WaitAsync(cancellationToken);
            
            if (!Directory.Exists(option.Path))
            {
                return;
            }

            logger.LogWarning("Clearing all files from local storage at: {Path}", option.Path);
            
            var directory = new DirectoryInfo(option.Path);
            foreach (var dir in directory.GetDirectories())
            {
                dir.Delete(true);
            }
            foreach (var file in directory.GetFiles())
            {
                file.Delete();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error clearing local storage at: {Path}", option.Path);
            throw;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task DeleteAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        var filePath = GetPath(fileEntry);
        if (!File.Exists(filePath))
        {
            logger.LogWarning("File not found for deletion: {Path}", filePath);
            return;
        }

        try
        {
            await _semaphore.WaitAsync(cancellationToken);
            File.Delete(filePath);
            logger.LogInformation("Deleted file: {Path}", filePath);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting file: {Path}", filePath);
            throw;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<FileResponse> DownloadAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        var filePath = GetPath(fileEntry);

        try
        {
            await _semaphore.WaitAsync(cancellationToken);
            var bytes = await File.ReadAllBytesAsync(filePath, cancellationToken);
            logger.LogInformation("Downloaded file: {Path}", filePath);
            return new(new MemoryStream(bytes), fileEntry.ContentType, filePath);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error downloading file: {Path}", filePath);
            throw;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public Task UnArchiveAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Unarchiving file: {FileName}", fileEntry.FileName);
        return Task.CompletedTask; // TODO: Implement local unarchiving
    }

    public async Task<FileResponse> UploadAsync(IFileEntry fileEntry, Stream stream, CancellationToken cancellationToken = default)
    {
        var filePath = Path.Combine(
            option.Path, 
            Guid.NewGuid().ToString("N") + Path.GetExtension(fileEntry.FileName)
        );

        try
        {
            await _semaphore.WaitAsync(cancellationToken);

            var folder = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrWhiteSpace(folder) && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
                logger.LogInformation("Created directory {Directory}", folder);
            }

            using var fileStream = new FileStream(filePath, FileMode.Create);
            await stream.CopyToAsync(fileStream, cancellationToken);

            logger.LogInformation("Uploaded file: {FilePath}", filePath);

            return new(Stream.Null, fileEntry.ContentType, filePath);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while uploading file: {FilePath}", filePath);
            throw;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private string GetPath(IFileEntry fileEntry) 
        => Path.Combine(option.Path, fileEntry.FileLocation);

    public void Dispose()
    {
        _semaphore.Dispose();
    }
}
