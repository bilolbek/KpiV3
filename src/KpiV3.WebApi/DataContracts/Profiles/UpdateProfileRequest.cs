using KpiV3.Domain.Employees.Commands;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Profiles;

public record UpdateProfileRequest
{
    [Required(AllowEmptyStrings = false)]
    public string FirstName { get; set; } = default!;

    [Required(AllowEmptyStrings = false)]
    public string LastName { get; set; } = default!;
    public string? MiddleName { get; set; }
    public Guid? AvatarId { get; set; }

    public UpdateProfileCommand ToCommand(Guid employeeId)
    {
        return new UpdateProfileCommand
        {
            EmployeeId = employeeId,
            AvatarId = AvatarId,
            Name = new()
            {
                FirstName = FirstName,
                LastName = LastName,
                MiddleName = MiddleName,
            },
        };
    }
}
