using KpiV3.Domain.Grades.Commands;

namespace KpiV3.WebApi.DataContracts.Grades;

public record CreateGradeRequest
{
    public Guid RequirementId { get; set; }
    public Guid EmployeeId { get; set; }
    public double Value { get; set; }

    public CreateGradeCommand ToCommand()
    {
        return new CreateGradeCommand
        {
            RequirementId = RequirementId,
            EmployeeId = EmployeeId,
            Value = Value,
        };
    }
}
