namespace KpiV3.Domain.Files.Ports;

public interface IFileStorage
{
    Task<Result<IError>> InitAsync();

    Task<Result<IError>> UploadAsync(Guid fileId, Stream content);

    Task<Result<Stream, IError>> DownloadAsync(Guid fileId);
}
