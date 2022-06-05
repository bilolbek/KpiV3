using KpiV3.Domain.Submissions.DataContracts;

namespace KpiV3.WebApi.DataContracts.Submissions;

public class SubmissionDto
{
    public SubmissionDto()
    {
    }

    public SubmissionDto(Submission submission)
    {
        Id = submission.Id;
        RequirementId = submission.RequirementId;
        FileId = submission.FileId;
        UploaderId = submission.UploaderId;
        Note = submission.Note;
        Status = submission.Status;
        SubmissionDate = submission.SubmissionDate;
    }

    public Guid Id { get; set; }
    public Guid RequirementId { get; set; }
    public Guid FileId { get; set; }
    public Guid UploaderId { get; set; }
    public string? Note { get; set; }
    public SubmissionStatus Status { get; set; }
    public DateTimeOffset SubmissionDate { get; set; }
}
