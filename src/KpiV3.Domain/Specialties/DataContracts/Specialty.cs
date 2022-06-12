using KpiV3.Domain.Positions.DataContracts;

namespace KpiV3.Domain.Specialties.DataContracts;

public class Specialty
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public Guid PositionId { get; set; }
    public Position Position { get; set; } = default!;
}
