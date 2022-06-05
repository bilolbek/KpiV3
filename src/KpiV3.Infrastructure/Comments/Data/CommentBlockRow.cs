using KpiV3.Domain.Comments.DataContracts;

namespace KpiV3.Infrastructure.Comments.Data;

internal class CommentBlockRow
{
    public CommentBlockRow()
    {
    }

    public CommentBlockRow(CommentBlock block)
    {
        Id = block.Id;
        Type = block.Type;
    }

    public Guid Id { get; set; }
    public CommentBlockType Type { get; set; }
}
