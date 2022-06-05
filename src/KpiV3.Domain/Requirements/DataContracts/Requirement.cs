namespace KpiV3.Domain.Requirements.DataContracts;

public record Requirement
{
    public Guid Id { get; set; }

    public Guid PeriodPartId { get; set; }
    public Guid SpecialtyId { get; set; }
    public Guid IndicatorId { get; set; }

    public double Weight { get; set; }

    public string? Note { get; set; }
}
