using KpiV3.Domain.Comments.Services;
using MediatR;

namespace KpiV3.Domain.Comments.Commands;

public record DeleteCommentCommand : IRequest
{
    public Guid CommentId { get; init; }
    public Guid IdOfWhoWantsToDelete { get; init; }
}

public class DeleteCommentCommandHandle : AsyncRequestHandler<DeleteCommentCommand>
{
    private readonly KpiContext _db;
    private readonly CommentAuthorityService _commentAuthorityService;

    public DeleteCommentCommandHandle(
        KpiContext db,
        CommentAuthorityService commentAuthorityService)
    {
        _db = db;
        _commentAuthorityService = commentAuthorityService;
    }

    protected override async Task Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        await _commentAuthorityService.EnsureEmployeeCanModifyCommentAsync(
            request.IdOfWhoWantsToDelete,
            request.CommentId,
            cancellationToken);

        var comment = await _db.Comments
            .FindAsync(new object?[] { request.CommentId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        _db.Comments.Remove(comment);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
