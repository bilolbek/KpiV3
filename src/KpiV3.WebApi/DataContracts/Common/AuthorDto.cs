using KpiV3.Domain.DataContracts.Models;

namespace KpiV3.WebApi.DataContracts.Common;

public class AuthorDto
{
    public AuthorDto()
    {
    }

    public AuthorDto(Author author)
    {
        Id = author.Id;
        Name = author.Name;
        AvatarId = author.AvatarId;
    }

    public Guid Id { get; set; }
    public Name Name { get; set; }
    public Guid? AvatarId { get; set; }
}
