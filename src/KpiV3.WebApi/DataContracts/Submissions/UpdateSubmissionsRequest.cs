using KpiV3.Domain.Submissions.Commands;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Submissions;

public class UpdateSubmissionsRequest
{
    [Required]
    public List<Guid> FileIds { get; init; } = default!;

    public UpdateSubmissionCommand ToCommand(Guid submissionId, Guid employeeId)
    {
        return new UpdateSubmissionCommand
        {
            EmployeeId = employeeId,
            FileIds = FileIds,
            SubmissionId = submissionId,
        };
    }
}
