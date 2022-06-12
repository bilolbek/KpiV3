using KpiV3.Domain.Posts.DataContracts;
using KpiV3.WebApi.DataContracts.Common;

namespace KpiV3.WebApi.DataContracts.Posts;

public class PostDto
{
    public PostDto(PostWithAuthor post)
    {
        Id = post.Id;
        Author = new(post.Author);
        Title = post.Title;
        Content = post.Content;
        CommentBlockId = post.CommentBlockId;
        WrittenDate = post.WrittenDate;
    }

    public Guid Id { get; init; }
    public ProfileDto Author { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string Content { get; init; } = default!;
    public Guid CommentBlockId { get; init; }
    public DateTimeOffset WrittenDate { get; init; }
}
