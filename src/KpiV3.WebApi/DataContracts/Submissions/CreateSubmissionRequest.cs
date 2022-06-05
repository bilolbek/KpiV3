using KpiV3.Domain.Submissions.Commands;

namespace KpiV3.WebApi.DataContracts.Submissions;

public record CreateSubmissionRequest
{
    public Guid RequirementId { get; set; }
    public Guid FileId { get; set; }
    public string? Note { get; set; }


    public CreateSubmissionCommand ToCommand(Guid employeeId)
    {
        return new CreateSubmissionCommand
        {
            EmployeeId = employeeId,
            RequirementId = RequirementId,
            FileId = FileId,
            Note = Note,
        };
    }
}
