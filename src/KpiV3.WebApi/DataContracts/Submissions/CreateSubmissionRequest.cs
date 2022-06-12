using KpiV3.Domain.Submissions.Commands;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Submissions;

public record CreateSubmissionRequest
{
    public Guid RequirementId { get; set; }

    [Required]
    public List<Guid> FileIds { get; set; } = default!;

    public CreateSubmissionCommand ToCommand(Guid employeeId)
    {
        return new CreateSubmissionCommand
        {
            EmployeeId = employeeId,
            FileIds = FileIds,
            RequirementId = RequirementId,
        };
    }
}
