namespace KpiV3.Domain.Comments.DataContracts;

public record Comment
{
    public Guid Id { get; set; }
    public string Content { get; set; } = default!;
    public Guid AuthorId { get; set; }
    public Guid BlockId { get; set; }
    public DateTimeOffset WrittenDate { get; set; }

    public Result<Comment, IError> CanBeModifiedBy(Guid modifierId)
    {
        if (modifierId == AuthorId)
        {
            return Result<Comment, IError>.Ok(this);
        }

        return Result<Comment, IError>.Fail(new ForbidenAction("You are not allowed to modify this comment"));
    }
}
