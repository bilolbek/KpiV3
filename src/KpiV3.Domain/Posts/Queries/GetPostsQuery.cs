using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Posts.DataContracts;
using MediatR;

namespace KpiV3.Domain.Posts.Queries;

public record GetPostsQuery : IRequest<Result<Page<PostWithAuthor>, IError>>
{
    public Pagination Pagination { get; set; }
}
