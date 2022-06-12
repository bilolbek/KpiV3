using KpiV3.Domain.Positions.Commands;

namespace KpiV3.WebApi.DataContracts.Positions;

public record UpdatePositionRequest
{
    public string Name { get; init; } = default!;

    public UpdatePositionCommand ToCommand(Guid positionId)
    {
        return new UpdatePositionCommand
        {
            PositionId = positionId,
            Name = Name,
        };
    }
}
