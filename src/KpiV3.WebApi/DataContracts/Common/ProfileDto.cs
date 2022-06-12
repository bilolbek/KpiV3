using KpiV3.Domain.Common.DataContracts;

namespace KpiV3.WebApi.DataContracts.Common;

public class ProfileDto
{
    public ProfileDto(Profile profile)
    {
        Id = profile.Id;
        Email = profile.Email;
        Name = profile.Name;
        AvatarId = profile.AvatarId;
    }

    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
    public Name Name { get; set; } = default!;
    public Guid? AvatarId { get; set; }
}