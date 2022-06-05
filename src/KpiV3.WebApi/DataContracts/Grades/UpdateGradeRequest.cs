using KpiV3.Domain.Grades.Commands;

namespace KpiV3.WebApi.DataContracts.Grades;

public class UpdateGradeRequest
{
    public Guid RequirementId { get; set; }
    public Guid EmployeeId { get; set; }
    public double Value { get; set; }

    public UpdateGradeCommand ToCommand()
    {
        return new UpdateGradeCommand
        {
            EmployeeId = EmployeeId,
            RequirementId = RequirementId,
            Value = Value,
        };
    }
}
