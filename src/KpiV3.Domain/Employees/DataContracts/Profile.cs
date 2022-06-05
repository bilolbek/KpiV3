using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Positions.DataContracts;

namespace KpiV3.Domain.Employees.DataContracts;

public class Profile
{
    public Guid Id { get; set; }

    public string Email { get; set; } = default!;
    public Name Name { get; set; }

    public Guid? AvatarId { get; set; }

    public Position Position { get; set; } = default!;
}
