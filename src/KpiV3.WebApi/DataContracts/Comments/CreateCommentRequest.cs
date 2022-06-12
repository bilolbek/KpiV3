using KpiV3.Domain.Comments.Commands;

namespace KpiV3.WebApi.DataContracts.Comments;

public class CreateCommentRequest
{
    public string Content { get; set; } = default!;
    public Guid BlockId { get; set; }

    public CreateCommentCommand ToCommand(Guid authorId)
    {
        return new CreateCommentCommand
        {
            EmployeeId = authorId,
            Content = Content,
            BlockId = BlockId,
        };
    }
}
