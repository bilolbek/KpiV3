using KpiV3.Domain.Employees.Queries;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Employees;

public record GetEmployeesRequest
{
    [FromQuery]
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; }

    [FromQuery]
    [Range(1, int.MaxValue)]
    public int PageSize { get; set; }

    [FromQuery]
    public Guid? PositionId { get; set; }

    public GetEmployeesQuery ToQuery()
    {
        return new GetEmployeesQuery
        {
            Pagination = new()
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
            },

            PositionId = PositionId,
        };
    }
}
