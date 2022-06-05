using KpiV3.Domain.Posts.DataContracts;

namespace KpiV3.Infrastructure.Posts.Data;

internal class PostWithAuthorRow
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public Guid CommentBlockId { get; set; }
    public DateTimeOffset WrittenDate { get; set; }

    public Guid AuthorId { get; set; }
    public string AuthorFirstName { get; set; } = default!;
    public string AuthorLastName { get; set; } = default!;
    public string? AuthorMiddleName { get; set; }
    public Guid? AuthorAvatarId { get; set; }

    public PostWithAuthor ToModel()
    {
        return new PostWithAuthor
        {
            Id = Id,
            Title = Title,
            Content = Content,
            CommentBlockId = CommentBlockId,
            WrittenDate = WrittenDate,
            Author = new()
            {
                Id = AuthorId,
                Name = new()
                {
                    FirstName = AuthorFirstName,
                    LastName = AuthorLastName,
                    MiddleName = AuthorMiddleName,
                },
                AvatarId = AuthorAvatarId,
            },
        };
    }
}
