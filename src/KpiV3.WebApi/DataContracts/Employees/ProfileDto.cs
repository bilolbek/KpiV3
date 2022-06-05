using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Employees.DataContracts;
using KpiV3.WebApi.DataContracts.Positions;

namespace KpiV3.WebApi.DataContracts.Employees;

public class ProfileDto
{
    public ProfileDto()
    {
    }

    public ProfileDto(Profile profile)
    {
        Id = profile.Id;
        Email = profile.Email;
        Name = profile.Name;
        AvatarId = profile.AvatarId;
        Position = new PositionDto(profile.Position);
    }

    public Guid Id { get; set; }

    public string Email { get; set; } = default!;
    public Name Name { get; set; }

    public Guid? AvatarId { get; set; }

    public PositionDto Position { get; set; } = default!;
}
