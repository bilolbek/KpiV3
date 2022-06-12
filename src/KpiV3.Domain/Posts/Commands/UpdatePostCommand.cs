using KpiV3.Domain.Posts.DataContracts;
using MediatR;

namespace KpiV3.Domain.Posts.Commands;

public record UpdatePostCommand : IRequest<PostWithAuthor>
{
    public Guid PostId { get; init; }
    public string Title { get; init; } = default!;
    public string Content { get; set; } = default!;
}

public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, PostWithAuthor>
{
    private readonly KpiContext _db;

    public UpdatePostCommandHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<PostWithAuthor> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _db.Posts
            .FindAsync(new object?[] { request.PostId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        post.Title = request.Title;
        post.Content = request.Content;

        await _db.SaveChangesAsync(cancellationToken);

        var author = await _db.Employees
            .FindAsync(new object?[] { post.AuthorId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        return new PostWithAuthor
        {
            Id = post.Id,
            Author = new()
            {
                Id = author.Id,
                Email = author.Email,
                Name = author.Name,
                AvatarId = author.AvatarId,
            },
            CommentBlockId = post.CommentBlockId,
            Title = post.Title,
            Content = post.Content,
            WrittenDate = post.WrittenDate,
        };
    }
}
