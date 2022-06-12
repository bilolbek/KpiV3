using KpiV3.Domain.Specialties.DataContracts;

namespace KpiV3.Domain.Positions.DataContracts;

public class Position
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
    public PositionType Type { get; set; } = default!;

    public ICollection<Specialty> Specialties { get; set; } = default!;
}
