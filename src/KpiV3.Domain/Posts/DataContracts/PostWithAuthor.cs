using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Employees.DataContracts;

namespace KpiV3.Domain.Posts.DataContracts;

public record PostWithAuthor
{
    public PostWithAuthor()
    {        
    }

    public PostWithAuthor(Post post, Employee author)
    {
        Id = post.Id;
        Author = new(author);
        Title = post.Title;
        Content = post.Content;
        CommentBlockId = post.CommentBlockId;
        WrittenDate = post.WrittenDate;
    }

    public Guid Id { get; set; }

    public Author Author { get; set; } = default!;

    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;

    public Guid CommentBlockId { get; set; }

    public DateTimeOffset WrittenDate { get; set; }
}
