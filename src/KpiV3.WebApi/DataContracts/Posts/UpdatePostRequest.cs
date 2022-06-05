using KpiV3.Domain.Posts.Commands;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Posts;

public record UpdatePostRequest
{
    [Required(AllowEmptyStrings = false)]
    public string Title { get; set; } = default!;

    [Required(AllowEmptyStrings = false)]
    public string Content { get; set; } = default!;

    public UpdatePostCommand ToCommand(Guid postId, Guid idOfWhoWantsToEdit)
    {
        return new UpdatePostCommand
        {
            PostId = postId,
            Title = Title,
            Content = Content,
            IdOfWhoWantsToEdit = idOfWhoWantsToEdit,
        };
    }
}
