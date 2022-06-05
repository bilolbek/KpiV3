using KpiV3.Domain.Employees.Commands;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Profiles;

public record ChangePasswordRequest
{
    [Required(AllowEmptyStrings = false)]
    public string OldPassword { get; set; } = default!;

    [Required(AllowEmptyStrings = false)]
    public string NewPassword { get; set; } = default!;

    public ChangePasswordCommand ToCommand(Guid employeeId)
    {
        return new ChangePasswordCommand
        {
            EmployeeId = employeeId,
            OldPassword = OldPassword,
            NewPassword = NewPassword,
        };
    }
}
