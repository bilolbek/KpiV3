using KpiV3.Domain.Posts.Queries;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Posts;

public class GetPostsRequest
{
    [FromQuery]
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; }

    [Range(1, int.MaxValue)]
    public int PageSize { get; set; }

    public GetPostsQuery ToQuery()
    {
        return new GetPostsQuery
        {
            Pagination = new()
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
            }
        };
    }
}
