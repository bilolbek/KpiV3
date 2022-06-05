using KpiV3.Domain.Comments.DataContracts;

namespace KpiV3.Infrastructure.Comments.Data;

internal class CommentRow
{
    public CommentRow()
    {
    }

    public CommentRow(Comment comment)
    {
        Id = comment.Id;
        Content = comment.Content;
        AuthorId = comment.AuthorId;
        BlockId = comment.BlockId;
        WrittenDate = comment.WrittenDate;
    }

    public Guid Id { get; set; }
    public string Content { get; set; } = default!;
    public Guid AuthorId { get; set; }
    public Guid BlockId { get; set; }
    public DateTimeOffset WrittenDate { get; set; }

    public Comment ToModel()
    {
        return new Comment
        {
            Id = Id,
            Content = Content,
            AuthorId = AuthorId,
            BlockId = BlockId,
            WrittenDate = WrittenDate,
        };
    }
}
