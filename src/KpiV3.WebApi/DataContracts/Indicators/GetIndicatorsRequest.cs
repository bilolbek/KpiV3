using KpiV3.Domain.Indicators.Queries;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Indicators;

public record GetIndicatorsRequest
{
    [FromQuery]
    public string? Name { get; init; }

    [FromQuery]
    public string? Description { get; init; }

    [FromQuery]
    public string? Comment { get; init; }

    [FromQuery]
    [Range(1, int.MaxValue)]
    public int PageNumber { get; init; }

    [FromQuery]
    [Range(1, int.MaxValue)]
    public int PageSize { get; init; }

    public GetIndicatorsQuery ToQuery()
    {
        return new GetIndicatorsQuery
        {
            Name = Name,
            Description = Description,
            Comment = Comment,
            Pagination = new()
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
            },
        };
    }
}
