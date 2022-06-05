using KpiV3.Domain.Files.Queries;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Files;

public record GetUploadedFilesRequest
{
    [FromQuery]
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; }

    [FromQuery]
    [Range(1, int.MaxValue)]
    public int PageSize { get; set; }

    public GetUploadedFilesQuery ToQuery(Guid employeeId)
    {
        return new GetUploadedFilesQuery
        {
            EmployeeId = employeeId,
            Pagination = new()
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
            },
        };
    }
}
