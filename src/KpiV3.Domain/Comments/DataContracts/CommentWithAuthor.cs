using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Employees.DataContracts;

namespace KpiV3.Domain.Comments.DataContracts;

public record CommentWithAuthor
{
    public CommentWithAuthor()
    {
    }

    public CommentWithAuthor(Comment comment, Employee author)
    {
        Id = comment.Id;
        Author = new(author);
        Content = comment.Content;
        WrittenDate = comment.WrittenDate;
        BlockId = comment.BlockId;
    }

    public Guid Id { get; set; }

    public Author Author { get; set; } = default!;

    public Guid BlockId { get; set; }

    public string Content { get; set; } = default!;

    public DateTimeOffset WrittenDate { get; set; }
}
