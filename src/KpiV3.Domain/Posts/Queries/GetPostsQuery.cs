using KpiV3.Domain.Common.DataContracts;
using KpiV3.Domain.Posts.DataContracts;
using MediatR;

namespace KpiV3.Domain.Posts.Queries;

public record GetPostsQuery : IRequest<Page<PostWithAuthor>>
{
    public string? Title { get; init; }
    public string? Content { get; init; }
    public Pagination Pagination { get; init; }
}

public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, Page<PostWithAuthor>>
{
    private readonly KpiContext _db;

    public GetPostsQueryHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<Page<PostWithAuthor>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Posts.Select(p => new PostWithAuthor
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
        });

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            query = query.Where(p => p.Title.Contains(request.Title));
        }

        if (!string.IsNullOrWhiteSpace(request.Content))
        {
            query = query.Where(p => p.Content.Contains(request.Content));
        }

        return await query.ToPageAsync(request.Pagination, cancellationToken);
    }
}
