using KpiV3.Domain.Posts.Queries;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Posts;

public record GetPostsRequest
{
    [FromQuery]
    public string? Title { get; init; }

    [FromQuery]
    public string? Content { get; init; }

    [FromQuery]
    [Range(1, int.MaxValue)]
    public int PageNumber { get; init; }

    [FromQuery]
    [Range(1, int.MaxValue)]
    public int PageSize { get; init; }

    public GetPostsQuery ToQuery()
    {
        return new GetPostsQuery
        {
            Title = Title,
            Content = Content,
            Pagination = new()
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
            },
        };
    }
}