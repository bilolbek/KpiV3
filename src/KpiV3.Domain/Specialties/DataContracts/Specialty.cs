namespace KpiV3.Domain.Specialties.DataContracts;

public record Specialty
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;

    public Guid PositionId { get; set; }
}
