using KpiV3.Domain.Positions.Queries;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Positions;

public class GetPositionsRequest
{
    [FromQuery]
    public string? Name { get; init; }

    [FromQuery]
    [Range(1, int.MaxValue)]
    public int PageNumber { get; init; }

    [FromQuery]
    [Range(1, int.MaxValue)]
    public int PageSize { get; init; }

    public GetPositionsQuery ToQuery()
    {
        return new GetPositionsQuery
        {
            Name = Name,
            Pagination = new()
            {
                PageNumber = PageNumber,
                PageSize = PageSize
            },
        };
    }
}
