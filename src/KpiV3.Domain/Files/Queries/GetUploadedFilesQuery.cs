using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Files.DataContracts;
using MediatR;

namespace KpiV3.Domain.Files.Queries;

public record GetUploadedFilesQuery : IRequest<Result<Page<FileMetadata>, IError>>
{
    public Guid EmployeeId { get; set; }
    public Pagination Pagination { get; set; }
}
