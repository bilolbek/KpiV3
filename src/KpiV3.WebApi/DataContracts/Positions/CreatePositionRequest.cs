using KpiV3.Domain.Positions.Commands;
using KpiV3.Domain.Positions.DataContracts;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Positions;

public record CreatePositionRequest
{
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; } = default!;

    public CreatePositionCommand ToCommand()
    {
        return new CreatePositionCommand
        {
            Name = Name,
            Type = PositionType.Employee,
        };
    }
}
