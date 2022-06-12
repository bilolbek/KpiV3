using KpiV3.Domain.Common.DataContracts;
using KpiV3.Domain.Files.DataContract;
using KpiV3.Domain.Grades.DataContracts;
using KpiV3.Domain.Positions.DataContracts;
using KpiV3.Domain.SpecialtyChoices.DataContracts;

namespace KpiV3.Domain.Employees.DataContracts;

public class Employee
{
    public Guid Id { get; set; }

    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;

    public Name Name { get; set; } = default!;

    public Guid? AvatarId { get; set; }
    public FileMetadata? Avatar { get; set; }

    public Guid PositionId { get; set; }
    public Position Position { get; set; } = default!;

    public DateTimeOffset RegisteredDate { get; set; }

    public bool IsBlocked { get; set; }

    public ICollection<SpecialtyChoice> SpecialtyChoices { get; set; } = default!;
    
    public ICollection<Grade> Grades { get; set; } = default!;
}
