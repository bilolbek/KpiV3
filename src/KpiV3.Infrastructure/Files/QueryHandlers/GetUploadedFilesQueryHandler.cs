using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Files.DataContracts;
using KpiV3.Domain.Files.Queries;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Files.Data;
using MediatR;

namespace KpiV3.Infrastructure.Files.QueryHandlers;

internal class GetUploadedFilesQueryHandler : IRequestHandler<GetUploadedFilesQuery, Result<Page<FileMetadata>, IError>>
{
    private readonly Database _db;

    public GetUploadedFilesQueryHandler(Database db)
    {
        _db = db;
    }

    public async Task<Result<Page<FileMetadata>, IError>> Handle(GetUploadedFilesQuery request, CancellationToken cancellationToken)
    {
        const string count = @"
SELECT COUNT(*) FROM files
WHERE uploader_id = @EmployeeId";

        const string select = @"
SELECT * FROM files
WHERE uploader_id = @EmployeeId
ORDER BY name
LIMIT @Limit OFFSET @Offset";

        return await _db
            .QueryFirstAsync<int>(new(count, new { request.EmployeeId }))
            .BindAsync(total => _db
                .QueryAsync<FileMetadataRow>(new(select, new
                {
                    request.EmployeeId,
                    request.Pagination.Limit,
                    request.Pagination.Offset
                }))
                .MapAsync(rows => new Page<FileMetadataRow>(total, request.Pagination, rows)))
            .MapAsync(rows => rows.Map(row => row.ToModel()));
    }
}
