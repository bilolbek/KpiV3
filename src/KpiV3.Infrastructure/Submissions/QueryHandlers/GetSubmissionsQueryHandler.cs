using KpiV3.Domain.Submissions.DataContracts;
using KpiV3.Domain.Submissions.Queries;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Submissions.Data;
using MediatR;

namespace KpiV3.Infrastructure.Submissions.QueryHandlers;

internal class GetSubmissionsQueryHandler : IRequestHandler<GetSubmissionsQuery, Result<List<Submission>, IError>>
{
    private readonly Database _db;

    public GetSubmissionsQueryHandler(Database db)
    {
        _db = db;
    }

    public async Task<Result<List<Submission>, IError>> Handle(GetSubmissionsQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
SELECT * FROM submissions
WHERE uploader_id = @EmployeeId AND requirement_id = @RequirementId";

        return await _db
            .QueryAsync<SubmissionRow>(new(sql, new { request.RequirementId, request.EmployeeId }))
            .MapAsync(rows => rows.Select(row => row.ToModel()).ToList());
    }
}
