using KpiV3.Domain.Employees.DataContracts;

namespace KpiV3.Infrastructure.Employees.Data;

public class PositionRow
{
    public PositionRow()
    {
    }

    public PositionRow(Position position)
    {
        Id = position.Id;
        Name = position.Name;
    }

    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

    public Position ToModel()
    {
        return new Position
        {
            Id = Id,
            Name = Name
        };
    }
}
