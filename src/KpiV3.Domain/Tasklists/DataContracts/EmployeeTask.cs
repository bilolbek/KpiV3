namespace KpiV3.Domain.Tasklists.DataContracts;

public class EmployeeTask
{
    public Guid RequirementId { get; init; }

    public Guid IndicatorId { get; init; }
    public string IndicatorName { get; init; } = default!;

    public Guid PeriodPartId { get; init; }
    public string PeriodPartName { get; init; } = default!;

    public double Weight { get; init; }
    public double? Grade { get; init; }

    public bool HasSubmission { get; init; }
}
