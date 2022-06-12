using KpiV3.Domain.Submissions.Commands;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Submissions;

public record CreateSubmissionRequest
{
    public Guid RequirementId { get; init; }

    [Required]
    public List<Guid> FileIds { get; init; } = default!;

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
