using KpiV3.Domain.Comments.DataContracts;

namespace KpiV3.Infrastructure.Comments.Data;

internal class CommentWithAuthorRow
{
    public Guid Id { get; set; }
    public string Content { get; set; } = default!;
    public DateTimeOffset WrittenDate { get; set; }
    public Guid BlockId { get; set; }

    public Guid AuthorId { get; set; }
    public string AuthorFirstName { get; set; } = default!;
    public string AuthorLastName { get; set; } = default!;
    public string? AuthorMiddleName { get; set; }
    public Guid? AuthorAvatarId { get; set; }

    public CommentWithAuthor ToModel()
    {
        return new CommentWithAuthor
        {
            Id = Id,
            Content = Content,
            WrittenDate = WrittenDate,
            BlockId = BlockId,

            Author = new()
            {
                Id = AuthorId,
                AvatarId = AuthorAvatarId,
                Name = new()
                {
                    FirstName = AuthorFirstName,
                    LastName = AuthorLastName,
                    MiddleName = AuthorMiddleName,
                },
            },
        };
    }
}
