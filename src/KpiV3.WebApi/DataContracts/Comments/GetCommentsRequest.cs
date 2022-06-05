using KpiV3.Domain.Comments.Queries;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Comments;

public class GetCommentsRequest
{
    [Range(1, int.MaxValue)]
    [FromQuery]
    public int PageNumber { get; set; }

    [Range(1, int.MaxValue)]
    [FromQuery]
    public int PageSize { get; set; }

    public GetCommentsQuery ToQuery(Guid blockId)
    {
        return new GetCommentsQuery
        {
            BlockId = blockId,
            Pagination = new()
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
            },
        };
    }
}
