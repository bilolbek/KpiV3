using KpiV3.Domain.Files.DataContracts;
using KpiV3.Domain.Files.Ports;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Files.Data;

namespace KpiV3.Infrastructure.Files.Repositories;

internal class FileMetadataRepository : IFileMetadataRepository
{
    private readonly Database _db;

    public FileMetadataRepository(Database db)
    {
        _db = db;
    }

    public async Task<Result<FileMetadata, IError>> FindByIdAsync(Guid metadataId)
    {
        const string sql = @"
SELECT * FROM files 
WHERE id = @metadataId";

        return await _db
            .QueryFirstAsync<FileMetadataRow>(new(sql, new { metadataId }))
            .MapAsync(row => row.ToModel());
    }

    public async Task<Result<IError>> InsertAsync(FileMetadata metadata)
    {
        const string sql = @"
INSERT INTO files (id, name, content_type, uploader_id, length)
VALUES (@Id, @Name, @ContentType, @UploaderId, @Length)";

        return await _db.ExecuteAsync(new(sql, new FileMetadataRow(metadata)));
    }
}
