using KpiV3.Domain.Comments.DataContracts;
using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Requirements.DataContracts;
using KpiV3.Domain.Submissions.DataContracts;

namespace KpiV3.WebApi.DataContracts.Submissions;

public class SubmissionDto
{
    public SubmissionDto(Submission submission)
    {
        Id = submission.Id;
        RequirementId = submission.RequirementId;
        EmployeeId = submission.EmployeeId;
        CommentBlockId = submission.CommentBlockId;
        SubmittedDate = submission.SubmittedDate;
        FileIds = submission.Files.Select(sf => sf.FileId).ToList();
    }

    public Guid Id { get; set; }

    public Guid RequirementId { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid CommentBlockId { get; set; }
    public DateTimeOffset SubmittedDate { get; set; }
    public List<Guid> FileIds { get; set; } = default!;
}
