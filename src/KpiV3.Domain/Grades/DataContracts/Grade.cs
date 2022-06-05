namespace KpiV3.Domain.Grades.DataContracts;

public record Grade
{
    public Guid RequirementId { get; set; }
    public Guid EmployeeId { get; set; }
    public double Value { get; set; }
}
