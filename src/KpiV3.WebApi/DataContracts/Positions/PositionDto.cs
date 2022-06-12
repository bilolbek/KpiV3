using KpiV3.Domain.Positions.DataContracts;

namespace KpiV3.WebApi.DataContracts.Positions;

public class PositionDto
{
    public PositionDto(Position position)
    {
        Id = position.Id;
        Name = position.Name;
        Type = position.Type;
    }

    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public PositionType Type { get; init; }
}
