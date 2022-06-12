namespace KpiV3.Domain.Requirements.DataContracts;

public record SpecialtyRequirement
{
    public Guid RequirementId { get; init; }

    public Guid IndicatorId { get; init; }
    public string IndicatorName { get; init; } = default!;

    public Guid PeriodPartId { get; init; }
    public string PeriodPartName { get; init; } = default!;

    public double Weight { get; init; }

    public string? Note { get; init; }
}