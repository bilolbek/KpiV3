using KpiV3.Domain.Posts.Commands;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Posts;

public record CreatePostRequest
{
    [Required(AllowEmptyStrings = false)]
    public string Title { get; set; } = default!;

    [Required(AllowEmptyStrings = false)]
    public string Content { get; set; } = default!;

    public CreatePostCommand ToCommand(Guid authorId)
    {
        return new CreatePostCommand
        {
            AuthorId = authorId,
            Title = Title,
            Content = Content,
        };
    }
}
