using KpiV3.Domain.Common.DataContracts;

namespace KpiV3.Domain.Posts.DataContracts;

public record PostWithAuthor
{
    public Guid Id { get; set; }
    public Profile Author { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public Guid CommentBlockId { get; set; }
    public DateTimeOffset WrittenDate { get; set; }
}
