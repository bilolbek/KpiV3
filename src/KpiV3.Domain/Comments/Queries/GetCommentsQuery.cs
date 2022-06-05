using KpiV3.Domain.Comments.DataContracts;
using KpiV3.Domain.DataContracts.Models;
using MediatR;

namespace KpiV3.Domain.Comments.Queries;

public record GetCommentsQuery : IRequest<Result<Page<CommentWithAuthor>, IError>>
{
    public Guid BlockId { get; set; }
    public Pagination Pagination { get; set; }
}
