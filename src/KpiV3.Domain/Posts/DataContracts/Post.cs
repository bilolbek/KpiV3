namespace KpiV3.Domain.Posts.DataContracts;

public record Post
{
    public Guid Id { get; set; }

    public Guid AuthorId { get; set; }

    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;

    public Guid CommentBlockId { get; set; }

    public DateTimeOffset WrittenDate { get; set; }

    public Result<Post, IError> CanBeModifiedBy(Guid modifierId)
    {
        if (modifierId == AuthorId)
            return Result<Post, IError>.Ok(this);

        return Result<Post, IError>.Fail(new ForbidenAction("You are not allowed to modify this post"));
    }
}
