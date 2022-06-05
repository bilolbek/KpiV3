using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Posts.DataContracts;
using KpiV3.WebApi.DataContracts.Common;

namespace KpiV3.WebApi.DataContracts.Posts;

public class PostDto
{
    public PostDto()
    {
    }

    public PostDto(PostWithAuthor post)
    {
        Id = post.Id;
        Author = new AuthorDto(post.Author);
        Title = post.Title;
        Content = post.Content;
        CommentBlockId = post.CommentBlockId;
        WrittenDate = post.WrittenDate;
    }

    public Guid Id { get; set; }

    public AuthorDto Author { get; set; } = default!;

    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;

    public Guid CommentBlockId { get; set; }

    public DateTimeOffset WrittenDate { get; set; }
}
