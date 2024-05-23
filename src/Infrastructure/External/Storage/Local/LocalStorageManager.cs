using eCommerceWeb.Domain.Primitives.Storage;

namespace eCommerceWeb.External.Storage.Local;

internal sealed class LocalStorageManager(LocalOption option) : IFileStorageManger
{
    public Task ArchiveAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask; // TODO: Implement local archiving
    }

    public async Task ClearAsync(CancellationToken cancellationToken = default)
    {
        await Task.Run(() =>
        {
            if (!Directory.Exists(option.Path)) return;
            foreach (var dir in new DirectoryInfo(option.Path).GetDirectories())
            {
                dir.Delete(true);
            }
        }, cancellationToken);
    }

    public async Task DeleteAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        await Task.Run(() =>
        {
            var filePath = GetPath(fileEntry);
            if (!File.Exists(filePath)) return;
            File.Delete(filePath);
        }, cancellationToken);
    }

    public async Task<byte[]> GetByteArrayAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        var filePath = GetPath(fileEntry);
        return await File.ReadAllBytesAsync(filePath, cancellationToken);
    }

    public async Task<Stream> GetStreamAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        var byteArray = await GetByteArrayAsync(fileEntry, cancellationToken);
        return new MemoryStream(byteArray);
    }

    public async Task<string> GetUriAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        return await Task.Run(() =>
        {
            var filePath = GetPath(fileEntry);
            return File.Exists(filePath) 
                ? filePath
                : string.Empty;
        }, cancellationToken);
    }

    public Task UnArchiveAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask; // TODO: Implement local unarchiving
    }

    public async Task UploadAsync(IFileEntry fileEntry, Stream stream, CancellationToken cancellationToken = default)
    {
        stream.Position = 0;
        string filePath = GetPath(fileEntry);

        var folder = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder!);

        using var fileStream = new FileStream(filePath, FileMode.Create);
        await stream.CopyToAsync(fileStream, cancellationToken);
    }

    private string GetPath(IFileEntry fileEntry) => 
        Path.Combine(option.Path, fileEntry.FileLocation + Path.GetExtension(fileEntry.FileName));
}
