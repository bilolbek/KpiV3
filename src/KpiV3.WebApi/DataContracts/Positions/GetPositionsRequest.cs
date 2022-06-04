using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Positions.Queries;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Positions;

public class GetPositionsRequest
{
    [FromQuery]
    [Range(1, int.MaxValue)]
    public int PageSize { get; set; }
    
    [FromQuery]
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; }

    [FromQuery]
    public string? Name { get; set; }

    public GetPositionsQuery ToQuery()
    {
        return new GetPositionsQuery
        {
            Pagination = new Pagination
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
            },

            Name = Name ?? ""
        };
    }
}
