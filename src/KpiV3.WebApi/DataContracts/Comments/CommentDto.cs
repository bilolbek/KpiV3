using KpiV3.Domain.Comments.DataContracts;
using KpiV3.WebApi.DataContracts.Common;

namespace KpiV3.WebApi.DataContracts.Comments;

public class CommentDto
{
    public CommentDto()
    {

    }

    public CommentDto(CommentWithAuthor         comment)
    {
        Id = comment.Id;
        Author = new(comment.Author);
        Content = comment.Content;
        WrittenDate = comment.WrittenDate;
        BlockId = comment.BlockId;
    }

    public Guid Id { get; set; }

    public AuthorDto Author { get; set; } = default!;

    public string Content { get; set; } = default!;

    public Guid BlockId { get; set; }

    public DateTimeOffset WrittenDate { get; set; }
}
