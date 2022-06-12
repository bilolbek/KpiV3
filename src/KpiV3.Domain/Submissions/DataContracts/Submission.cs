using KpiV3.Domain.Comments.DataContracts;
using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Files.DataContract;
using KpiV3.Domain.Requirements.DataContracts;

namespace KpiV3.Domain.Submissions.DataContracts;

public class Submission
{
    public Guid Id { get; set; }

    public Guid RequirementId { get; set; }
    public Requirement Requirement { get; set; } = default!;

    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = default!;

    public DateTimeOffset SubmittedDate { get; set; }

    public Guid CommentBlockId { get; set; }
    public CommentBlock CommentBlock { get; set; } = default!;

    public ICollection<SubmissionFile> Files { get; set; } = default!;
}
