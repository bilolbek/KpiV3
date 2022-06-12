using KpiV3.Domain.Posts.Commands;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Posts;

public record UpdatePostRequest
{
    [Required(AllowEmptyStrings = false)]
    public string Title { get; init; } = default!;

    [Required(AllowEmptyStrings = false)]
    public string Content { get; init; } = default!;

    public UpdatePostCommand ToCommand(Guid postId)
    {
        return new UpdatePostCommand
        {
            PostId = postId,
            Title = Title,
            Content = Content,
        };
    }
}
