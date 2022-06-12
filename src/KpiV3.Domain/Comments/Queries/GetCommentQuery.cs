using KpiV3.Domain.Comments.DataContracts;
using MediatR;

namespace KpiV3.Domain.Comments.Queries;

public record GetCommentQuery : IRequest<CommentWithAuthor>
{
    public Guid CommentId { get; init; }
}

public class GetCommentQueryHandler : IRequestHandler<GetCommentQuery, CommentWithAuthor>
{
    private readonly KpiContext _db;

    public GetCommentQueryHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<CommentWithAuthor> Handle(GetCommentQuery request, CancellationToken cancellationToken)
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
            .FirstOrDefaultAsync(c => c.Id == request.CommentId, cancellationToken)
            .EnsureFoundAsync();
    }
}
