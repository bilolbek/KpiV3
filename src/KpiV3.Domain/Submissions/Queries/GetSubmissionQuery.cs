using KpiV3.Domain.Submissions.DataContracts;
using MediatR;

namespace KpiV3.Domain.Submissions.Queries;

public record GetSubmissionQuery : IRequest<Submission>
{
    public Guid RequirementId { get; init; }
    public Guid EmployeeId { get; init; }
}

public class GetSubmissionsQueryHandler : IRequestHandler<GetSubmissionQuery, Submission>
{
    private readonly KpiContext _db;

    public GetSubmissionsQueryHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<Submission> Handle(GetSubmissionQuery request, CancellationToken cancellationToken)
    {
        return await _db.Submissions
            .FirstOrDefaultAsync(s =>
                s.RequirementId == request.RequirementId &&
                s.EmployeeId == request.EmployeeId, cancellationToken)
            .EnsureFoundAsync();
    }
}
