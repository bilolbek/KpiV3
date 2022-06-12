using KpiV3.Domain.Posts.Commands;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Posts;

public record CreatePostRequest
{
    [Required(AllowEmptyStrings = false)]
    public string Title { get; init; } = default!;

    [Required(AllowEmptyStrings = false)]
    public string Content { get; init; } = default!;

    public CreatePostCommand ToCommand(Guid employeeId)
    {
        return new CreatePostCommand
        {
            EmployeeId = employeeId,
            Title = Title,
            Content = Content,
        };
    }
}
