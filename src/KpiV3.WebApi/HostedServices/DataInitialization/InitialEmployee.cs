using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Employees.Commands;

namespace KpiV3.WebApi.HostedServices.DataInitialization;

public class InitialEmployee
{
    public string Email { get; set; } = default!;

    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? MiddleName { get; set; }

    public string Position { get; set; } = default!;

    public RegisterEmployeeCommand ToCreateCommand(Guid positionId)
    {
        return new RegisterEmployeeCommand
        {
            Email = Email,
            
            Name = new()
            {
                FirstName = FirstName,
                LastName = LastName,
                MiddleName = MiddleName,
            },

            PositionId = positionId,
        };
    }
}
