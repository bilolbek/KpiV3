using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Requirements.DataContracts;

namespace KpiV3.Domain.Grades.DataContracts;

public class Grade
{
    public Guid RequirementId { get; set; }
    public Requirement Requirement { get; set; } = default!;

    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = default!;

    public double Value { get; set; }
    public DateTimeOffset GradedDate { get; set; }
}
