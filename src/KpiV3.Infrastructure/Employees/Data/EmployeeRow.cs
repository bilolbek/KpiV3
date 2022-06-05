using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Employees.DataContracts;

namespace KpiV3.Infrastructure.Employees.Data;

internal class EmployeeRow
{
    public EmployeeRow()
    {
    }

    public EmployeeRow(Employee employee)
    {
        Id = employee.Id;
        Email = employee.Email;
        FirstName = employee.Name.FirstName;
        LastName = employee.Name.LastName;
        MiddleName = employee.Name.MiddleName;
        PasswordHash = employee.PasswordHash;
        PositionId = employee.PositionId;
        RegDate = employee.RegistrationDate;
        AvatarId = employee.AvatarId;
    }

    public Guid Id { get; set; }

    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? MiddleName { get; set; }

    public string PasswordHash { get; set; } = default!;

    public Guid PositionId { get; set; }

    public DateTimeOffset RegDate { get; set; }
    public Guid? AvatarId { get; set; }

    public Employee ToModel()
    {
        return new Employee
        {
            Id = Id,
            Email = new(Email),
            Name = new Name
            {
                FirstName = FirstName,
                LastName = LastName,
                MiddleName = MiddleName,
            },
            PasswordHash = new(PasswordHash),
            PositionId = PositionId,
            RegistrationDate = RegDate,
            AvatarId = AvatarId,
        };
    }
}
