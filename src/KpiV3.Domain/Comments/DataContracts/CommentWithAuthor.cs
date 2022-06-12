using KpiV3.Domain.Common.DataContracts;

namespace KpiV3.Domain.Comments.DataContracts;

public record CommentWithAuthor
{
    public Guid Id { get; init; }

    public Profile Author { get; init; } = default!;

    public string Content { get; init; } = default!;

    public DateTimeOffset WrittenDate { get; init; }

    public Guid CommentBlockId { get; set; }
}
