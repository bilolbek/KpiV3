using KpiV3.Domain.Common;
using KpiV3.Domain.Files.DataContracts;
using KpiV3.Domain.Files.Ports;

namespace KpiV3.Domain.Files;

public class FileService
{
    private readonly IFileMetadataRepository _repository;
    private readonly IFileStorage _storage;
    private readonly IGuidProvider _guidProvider;

    public FileService(
        IFileMetadataRepository repository,
        IFileStorage storage,
        IGuidProvider guidProvider)
    {
        _repository = repository;
        _storage = storage;
        _guidProvider = guidProvider;
    }

    public async Task<Result<FileMetadata, IError>> UploadAsync(FileToUpload fileToUpload)
    {
        var metadata = new FileMetadata
        {
            Id = _guidProvider.New(),
            Name = fileToUpload.Name,
            ContentType = fileToUpload.ContentType,
            Length = fileToUpload.Length,
            UploaderId = fileToUpload.UploaderId,
        };

        return await _repository
            .InsertAsync(metadata)
            .BindAsync(() => _storage.UploadAsync(metadata.Id, fileToUpload.Content))
            .InsertSuccessAsync(() => metadata);
    }

    public async Task<Result<FullFile, IError>> DownloadAsync(Guid fileId)
    {
        return await _repository
            .FindByIdAsync(fileId)
            .BindAsync(metadata => _storage
                .DownloadAsync(metadata.Id)
                .MapAsync(content => new FullFile
                {
                    Id = metadata.Id,
                    Content = content,
                    ContentType = metadata.ContentType,
                    Length = metadata.Length,
                    Name = metadata.Name,
                }));
    }
}
