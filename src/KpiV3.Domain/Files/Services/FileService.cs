using KpiV3.Domain.Files.DataContract;
using KpiV3.Domain.Files.Ports;

namespace KpiV3.Domain.Files.Services;

public class FileService
{
    private readonly KpiContext _db;
    private readonly IFileStorage _fileStorage;
    private readonly IGuidProvider _guidProvider;

    public FileService(
        KpiContext db,
        IFileStorage fileStorage,
        IGuidProvider guidProvider)
    {
        _db = db;
        _fileStorage = fileStorage;
        _guidProvider = guidProvider;
    }

    public async Task<UploadFileResponse> UploadFileAsync(
        UploadFileRequest request,
        CancellationToken cancellationToken = default)
    {
        var metadata = new FileMetadata
        {
            Id = _guidProvider.New(),
            Name = request.Name,
            ContentType = request.ContentType,
            Length = request.Length,
            OwnerId = request.OwnerId,
        };

        _db.Files.Add(metadata);

        await _db.SaveChangesAsync(cancellationToken);

        await _fileStorage.UploadAsync(metadata.Id, request.Content);

        return new UploadFileResponse
        {
            FileId = metadata.Id,
        };
    }

    public async Task<DownloadFileResponse> DownloadFileAsync(
        DownloadFileRequest request,
        CancellationToken cancellationToken = default)
    {
        var metadata = await _db.Files
            .FindAsync(new object?[] { request.FileId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        var content = await _fileStorage.DownloadAsync(metadata.Id);

        return new DownloadFileResponse
        {
            Id = metadata.Id,
            Name = metadata.Name,
            ContentType = metadata.ContentType,
            Content = content,
            Length = metadata.Length,
        };
    }
}
