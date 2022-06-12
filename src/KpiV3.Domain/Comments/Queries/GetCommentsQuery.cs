using KpiV3.Domain.Comments.DataContracts;
using KpiV3.Domain.Common.DataContracts;
using MediatR;

namespace KpiV3.Domain.Comments.Queries;

public record GetCommentsQuery : IRequest<Page<CommentWithAuthor>>
{
    public Guid BlockId { get; init; }
    public Pagination Pagination { get; init; }
}

public class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery, Page<CommentWithAuthor>>
{
    private readonly KpiContext _db;

    public GetCommentsQueryHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<Page<CommentWithAuthor>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        return await _db.Comments
            .Select(c => new CommentWithAuthor
            {
                Id = c.Id,
                Author = new()
                {
                    Id = c.AuthorId,
                    Email = c.Author.Email,
                    Name = c.Author.Name,
                    AvatarId = c.Author.AvatarId,
                },
                Content = c.Content,
                WrittenDate = c.WrittenDate,
                CommentBlockId = c.CommentBlockId,
            })
            .Where(c => c.CommentBlockId == request.BlockId)
            .ToPageAsync(request.Pagination, cancellationToken);
    }
}
