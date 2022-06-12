using KpiV3.Domain.Periods.Queries;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Periods;

public record GetPeriodsRequest
{
    [FromQuery]
    public string? Name { get; init; }

    [Range(1, int.MaxValue)]
    [FromQuery]
    public int PageNumber { get; init; }

    [Range(1, int.MaxValue)]
    [FromQuery]
    public int PageSize { get; init; }

    public GetPeriodsQuery ToQuery()
    {
        return new GetPeriodsQuery
        {
            Name = Name,
            Pagination = new()
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
            },
        };
    }
}
