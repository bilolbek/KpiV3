using KpiV3.Domain.Periods.Queries;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Periods;

public record GetPeriodsRequest
{
    [FromQuery]
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; }

    [FromQuery]
    [Range(1, int.MaxValue)]
    public int PageSize { get; set; }

    public GetPeriodsQuery ToQuery()
    {
        return new GetPeriodsQuery
        {
            Pagination = new()
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
            },
        };
    }
}
