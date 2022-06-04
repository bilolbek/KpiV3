using KpiV3.Domain.Positions.DataContracts;

namespace KpiV3.WebApi.DataContracts.Positions;

public record PositionDto
{
    public PositionDto()
    {
    }

    public PositionDto(Position position)
    {
        Id = position.Id;
        Name = position.Name;
    }

    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
