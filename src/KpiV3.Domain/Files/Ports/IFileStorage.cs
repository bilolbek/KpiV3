namespace KpiV3.Domain.Files.Ports;

public interface IFileStorage
{
    Task UploadAsync(Guid fileId, Stream content);
    Task<Stream> DownloadAsync(Guid fileId);
}
