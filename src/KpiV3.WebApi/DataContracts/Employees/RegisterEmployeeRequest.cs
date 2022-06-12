using KpiV3.Domain.Employees.Commands;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Employees;

public record RegisterEmployeeRequest
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = default!;

    [Required]
    public string FirstName { get; set; } = default!;
    
    [Required]
    public string LastName { get; set; } = default!;
    
    public string? MiddleName { get; set; }

    public Guid PositionId { get; set; }

    public CreateEmployeeCommand ToCommand()
    {
        return new CreateEmployeeCommand
        {
            Email = Email,
            PositionId = PositionId,
            Name = new()
            {
                FirstName = FirstName,
                LastName = LastName,
                MiddleName = MiddleName,
            },
        };
    }
}