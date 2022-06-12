using KpiV3.Domain.Comments.DataContracts;
using KpiV3.Domain.Posts.DataContracts;
using MediatR;

namespace KpiV3.Domain.Posts.Commands;

public record CreatePostCommand : IRequest<PostWithAuthor>
{
    public Guid EmployeeId { get; init; }
    public string Title { get; init; } = default!;
    public string Content { get; init; } = default!;
}

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostWithAuthor>
{
    private readonly KpiContext _db;
    private readonly IGuidProvider _guidProvider;
    private readonly IDateProvider _dateProvider;

    public CreatePostCommandHandler(
        KpiContext db,
        IGuidProvider guidProvider,
        IDateProvider dateProvider)
    {
        _db = db;
        _guidProvider = guidProvider;
        _dateProvider = dateProvider;
    }

    public async Task<PostWithAuthor> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var post = new Post
        {
            Id = _guidProvider.New(),
            AuthorId = request.EmployeeId,
            Title = request.Title,
            Content = request.Content,
            WrittenDate = _dateProvider.Now(),
            CommentBlock = new CommentBlock
            {
                Id = _guidProvider.New(),
                Type = CommentBlockType.Post,
            },
        };

        _db.Posts.Add(post);

        await _db.SaveChangesAsync(cancellationToken);

        var author = await _db.Employees
            .FindAsync(new object?[] { request.EmployeeId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        return new PostWithAuthor
        {
            Id = post.Id,
            Author = new()
            {
                Id = author.Id,
                Email = author.Email,
                Name = new()
                {
                    FirstName = author.Name.FirstName,
                    LastName = author.Name.LastName,
                    MiddleName = author.Name.MiddleName,
                },
                AvatarId = author.AvatarId,
            },
            CommentBlockId = post.CommentBlockId,
            Title = post.Title,
            Content = post.Content,
            WrittenDate = post.WrittenDate,
        };
    }
}
