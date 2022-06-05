using KpiV3.Domain.Grades.DataContracts;

namespace KpiV3.Infrastructure.Grades.Data;

internal class GradeRow
{
    public GradeRow()
    {
    }

    public GradeRow(Grade grade)
    {
        EmployeeId = grade.EmployeeId;
        RequirementId = grade.RequirementId;
        Value = grade.Value;
    }

    public Guid EmployeeId { get; set; }
    public Guid RequirementId { get; set; }
    public double Value { get; set; }

    public Grade ToModel()
    {
        return new Grade
        {
            RequirementId = RequirementId,
            EmployeeId = EmployeeId,
            Value = Value,
        };
    }
}
