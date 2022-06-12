using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Posts.DataContracts;

namespace KpiV3.Domain.Comments.DataContracts;

public class Comment
{
    public Guid Id { get; set; }

    public Guid AuthorId { get; set; }
    public Employee Author { get; set; } = default!;

    public string Content { get; set; } = default!;

    public Guid CommentBlockId { get; set; }
    public CommentBlock CommentBlock { get; set; } = default!;

    public DateTimeOffset WrittenDate { get; set; }
}
