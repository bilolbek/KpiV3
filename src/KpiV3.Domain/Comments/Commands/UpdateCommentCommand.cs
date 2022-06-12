using KpiV3.Domain.Comments.DataContracts;
using KpiV3.Domain.Comments.Services;
using MediatR;

namespace KpiV3.Domain.Comments.Commands;

public record UpdateCommentCommand : IRequest<CommentWithAuthor>
{
    public Guid CommentId { get; init; }
    public string Content { get; init; } = default!;
    public Guid IdOfWhoWantsToUpdate { get; init; }
}

public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, CommentWithAuthor>
{
    private readonly KpiContext _db;
    private readonly CommentAuthorityService _commentAuthorityService;

    public UpdateCommentCommandHandler(
        KpiContext db,
        CommentAuthorityService commentAuthorityService)
    {
        _db = db;
        _commentAuthorityService = commentAuthorityService;
    }

    public async Task<CommentWithAuthor> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        await _commentAuthorityService.EnsureEmployeeCanModifyCommentAsync(
            request.IdOfWhoWantsToUpdate,
            request.CommentId,
            cancellationToken);

        var comment = await _db.Comments
            .FindAsync(new object?[] { request.CommentId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        comment.Content = request.Content;

        await _db.SaveChangesAsync(cancellationToken);

        var author = await _db.Employees
            .FindAsync(new object?[] { comment.AuthorId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        return new CommentWithAuthor
        {
            Id = comment.Id,
            Author = new()
            {
                Id = author.Id,
                AvatarId = author.AvatarId,
                Email = author.Email,
                Name = author.Name,
            },
            CommentBlockId = comment.CommentBlockId,
            Content = comment.Content,
            WrittenDate = comment.WrittenDate,
        };
    }
}
