using KpiV3.Domain.Indicators.Queries;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Indicators;

public class GetIndicatorsRequest
{
    [FromQuery]
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; }

    [FromQuery]
    [Range(1, int.MaxValue)]
    public int PageSize { get; set; }

    public GetIndicatorsQuery ToQuery()
    {
        return new GetIndicatorsQuery
        {
            Pagination = new()
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
            },
        };
    }
}
