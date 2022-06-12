using KpiV3.Domain.Comments.Queries;
using KpiV3.Domain.Tasklists.Queries;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Tasklists;

public class GetAdminTasklistRequest
{
    [Range(1, int.MaxValue)]
    [FromQuery]
    public int PageNumber { get; set; }

    [Range(1, int.MaxValue)]
    [FromQuery]
    public int PageSize { get; set; }

    public GetAdminTasklistQuery ToQuery()
    {
        return new GetAdminTasklistQuery
        {
            Pagination = new()
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
            },
        };
    }
}
