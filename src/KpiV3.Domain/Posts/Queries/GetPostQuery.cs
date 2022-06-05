using KpiV3.Domain.Posts.DataContracts;
using MediatR;

namespace KpiV3.Domain.Posts.Queries;

public record GetPostQuery : IRequest<Result<PostWithAuthor, IError>>
{
    public Guid PostId { get; set; }
}
