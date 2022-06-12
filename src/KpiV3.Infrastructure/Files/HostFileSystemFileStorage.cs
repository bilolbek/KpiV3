using KpiV3.Domain.Files.Ports;

namespace KpiV3.Infrastructure.Files;

public class HostFileSystemFileStorage : IFileStorage
{
    private readonly string _storageDirectory;

    public HostFileSystemFileStorage(string storageDirectory)
    {
        _storageDirectory = storageDirectory;
    }

    public async Task UploadAsync(Guid fileId, Stream content)
    {
        await using var fileStream = File.OpenWrite(GetPathForFileId(fileId));
        await content.CopyToAsync(fileStream);
    }

    public Task<Stream> DownloadAsync(Guid fileId)
    {
        return Task.FromResult(File.OpenRead(GetPathForFileId(fileId)) as Stream);
    }

    private string GetPathForFileId(Guid fileId)
    {
        return Path.Combine(_storageDirectory, $"file_{fileId}.obj");
    }
}
