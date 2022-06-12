using KpiV3.Domain.Employees.Commands;
using KpiV3.Domain.Employees.DataContracts;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Employees;

public record CsvImportedEmployee
{
    [Required(AllowEmptyStrings = false)]
    public string Email { get; set; } = default!;
    
    [Required(AllowEmptyStrings = false)]
    public string FirstName { get; set; } = default!;

    [Required(AllowEmptyStrings = false)]
    public string LastName { get; set; } = default!;

    public string? MiddleName { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string Position { get; set; } = default!;

    public ImportedEmployee ToRegisterEmployee()
    {
        return new ImportedEmployee()
        {
            Email = Email.Trim(),
            Name = new()
            {
                FirstName = FirstName.Trim(),
                LastName = LastName.Trim(),
                MiddleName = MiddleName?.Trim(),
            },
            Position = Position.Trim(),
        };
    }
}
