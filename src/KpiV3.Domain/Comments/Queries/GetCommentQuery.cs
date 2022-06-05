using KpiV3.Domain.Comments.DataContracts;
using MediatR;

namespace KpiV3.Domain.Comments.Queries;

public record GetCommentQuery : IRequest<Result<CommentWithAuthor, IError>>
{
    public Guid CommentId { get; set; }
}
