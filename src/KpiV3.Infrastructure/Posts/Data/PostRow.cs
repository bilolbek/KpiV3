using KpiV3.Domain.Posts.DataContracts;

namespace KpiV3.Infrastructure.Posts.Data;

internal class PostRow
{
    public PostRow()
    {
    }

    public PostRow(Post post)
    {
        Id = post.Id;
        AuthorId = post.AuthorId;
        Title = post.Title;
        Content = post.Content;
        CommentBlockId = post.CommentBlockId;
        WrittenDate = post.WrittenDate;
    }

    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public Guid CommentBlockId { get; set; }
    public DateTimeOffset WrittenDate { get; set; }

    public Post ToModel()
    {
        return new Post
        {
            Id = Id,
            AuthorId = AuthorId,
            Title = Title,
            Content = Content,
            CommentBlockId = CommentBlockId,
            WrittenDate = WrittenDate,
        };
    }
}
