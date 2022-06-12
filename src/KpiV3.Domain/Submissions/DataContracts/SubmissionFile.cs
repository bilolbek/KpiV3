using KpiV3.Domain.Files.DataContract;


namespace KpiV3.Domain.Submissions.DataContracts;

public class SubmissionFile
{
    public Guid FileId { get; set; }
    public FileMetadata File { get; set; } = default!;

    public Guid SubmissionId { get; set; }
    public Submission Submission { get; set; } = default!;
}
