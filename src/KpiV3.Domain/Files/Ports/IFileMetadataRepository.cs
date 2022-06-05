using KpiV3.Domain.Files.DataContracts;

namespace KpiV3.Domain.Files.Ports;

public interface IFileMetadataRepository
{
    Task<Result<FileMetadata, IError>> FindByIdAsync(Guid metadataId);

    Task<Result<IError>> InsertAsync(FileMetadata metadata);
}
