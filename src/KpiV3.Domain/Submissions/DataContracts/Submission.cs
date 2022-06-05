namespace KpiV3.Domain.Submissions.DataContracts;

public record Submission
{
    public Guid Id { get; set; }

    public Guid RequirementId { get; set; }

    public Guid FileId { get; set; }
    public Guid UploaderId { get; set; }

    public string? Note { get; set; }

    public SubmissionStatus Status { get; set; }
    public DateTimeOffset SubmissionDate { get; set; }
}
