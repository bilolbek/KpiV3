using KpiV3.Domain.Submissions.Commands;

namespace KpiV3.WebApi.DataContracts.Submissions;

public record UpdateSubmissionRequest
{
    public Guid FileId { get; set; }
    public string? Note { get; set; }

    public UpdateSubmissionCommand ToCommand(Guid employeeId, Guid submissionId)
    {
        return new UpdateSubmissionCommand
        {
            IdOfWhoWantsToUpdate = employeeId,
            FileId = FileId,
            Note = Note,
            SubmissionId = submissionId,
        };
    }
}
