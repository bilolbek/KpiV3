using KpiV3.Domain.DataContracts.Models;

namespace KpiV3.Domain.Employees.DataContracts;

public record Employee
{
    public Guid Id { get; set; }

    public string Email { get; set; } = default!;
    public Name Name { get; set; }
    public string PasswordHash { get; set; } = default!;

    public Guid PositionId { get; set; }
    public Guid? AvatarId { get; set; }

    public DateTimeOffset RegistrationDate { get; set; }
}
