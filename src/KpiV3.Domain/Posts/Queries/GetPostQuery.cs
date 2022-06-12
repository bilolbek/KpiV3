using KpiV3.Domain.Posts.DataContracts;
using MediatR;

namespace KpiV3.Domain.Posts.Queries;

public record GetPostQuery : IRequest<PostWithAuthor>
{
    public Guid PostId { get; init; }
}

public class GetPostQueryHandler : IRequestHandler<GetPostQuery, PostWithAuthor>
{
    private readonly KpiContext _db;

    public GetPostQueryHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<PostWithAuthor> Handle(GetPostQuery request, CancellationToken cancellationToken)
    {
        return await _db.Posts
            .Select(p => new PostWithAuthor
            {
                Id = p.Id,
                Author = new()
                {
                    Id = p.AuthorId,
                    Email = p.Author.Email,
                    Name = p.Author.Name,
                    AvatarId = p.Author.AvatarId,
                },
                Title = p.Title,
                Content = p.Content,
                WrittenDate = p.WrittenDate,
                CommentBlockId = p.CommentBlockId,
            })
            .FirstOrDefaultAsync(p => p.Id == request.PostId, cancellationToken)
            .EnsureFoundAsync();
    }
}
