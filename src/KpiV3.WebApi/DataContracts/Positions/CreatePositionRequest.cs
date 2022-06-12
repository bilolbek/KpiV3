using KpiV3.Domain.Positions.Commands;
using KpiV3.Domain.Positions.DataContracts;

namespace KpiV3.WebApi.DataContracts.Positions;

public record CreatePositionRequest
{
    public string Name { get; init; } = default!;

    public CreatePositionCommand ToCommand()
    {
        return new CreatePositionCommand
        {
            Name = Name,
            Type = PositionType.Employee,
        };
    }
}
