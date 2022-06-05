using KpiV3.Domain.Comments.Commands;

namespace KpiV3.WebApi.DataContracts.Comments;

public record UpdateCommentRequest
{
    public string Content { get; set; } = default!;

    public UpdateCommentCommand ToCommand(Guid commentId, Guid idOfWhoWantsToEdit)
    {
        return new UpdateCommentCommand
        {
            CommentId = commentId,
            Content = Content,
            IdOfWhoWantsToUpdate = idOfWhoWantsToEdit,
        };
    }
}
