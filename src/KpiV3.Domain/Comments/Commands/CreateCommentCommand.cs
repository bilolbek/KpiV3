using KpiV3.Domain.Comments.DataContracts;
using MediatR;

namespace KpiV3.Domain.Comments.Commands;

public record CreateCommentCommand : IRequest<CommentWithAuthor>
{
    public Guid EmployeeId { get; init; }
    public string Content { get; init; } = default!;
    public Guid BlockId { get; init; }
}

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CommentWithAuthor>
{
    private readonly KpiContext _db;
    private readonly IGuidProvider _guidProvider;
    private readonly IDateProvider _dateProvider;

    public CreateCommentCommandHandler(
        KpiContext db,
        IGuidProvider guidProvider,
        IDateProvider dateProvider)
    {
        _db = db;
        _guidProvider = guidProvider;
        _dateProvider = dateProvider;
    }

    public async Task<CommentWithAuthor> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = new Comment
        {
            Id = _guidProvider.New(),
            AuthorId = request.EmployeeId,
            CommentBlockId = request.BlockId,
            Content = request.Content,
            WrittenDate = _dateProvider.Now(),
        };

        _db.Comments.Add(comment);

        await _db.SaveChangesAsync(cancellationToken);

        var author = await _db.Employees
            .FindAsync(new object?[] { request.EmployeeId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        return new CommentWithAuthor
        {
            Id = comment.Id,
            Author = new()
            {
                Id = author.Id,
                AvatarId = author.AvatarId,
                Email = author.Email,
                Name = new()
                {
                    FirstName = author.Name.FirstName,
                    LastName = author.Name.LastName,
                    MiddleName = author.Name.MiddleName,
                },
            },
            CommentBlockId = comment.CommentBlockId,
            Content = comment.Content,
            WrittenDate = comment.WrittenDate,
        };
    }
}
