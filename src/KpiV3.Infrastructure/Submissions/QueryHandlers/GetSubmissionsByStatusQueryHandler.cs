using Dapper;
using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Submissions.DataContracts;
using KpiV3.Domain.Submissions.Queries;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Submissions.Data;
using MediatR;

namespace KpiV3.Infrastructure.Submissions.QueryHandlers;

internal class GetSubmissionsByStatusQueryHandler : IRequestHandler<GetSubmissionsByStatusQuery, Result<Page<Submission>, IError>>
{
    private readonly Database _db;

    public GetSubmissionsByStatusQueryHandler(Database db)
    {
        _db = db;
    }

    public async Task<Result<Page<Submission>, IError>> Handle(GetSubmissionsByStatusQuery request, CancellationToken cancellationToken)
    {
        return await _db
            .QueryFirstAsync<int>(Count(request))
            .BindAsync(total => _db
                .QueryAsync<SubmissionRow>(Select(request))
                .MapAsync(rows => new Page<SubmissionRow>(total, request.Pagination, rows)))
            .MapAsync(rows => rows.Map(row => row.ToModel()));
    }

    private CommandDefinition Count(GetSubmissionsByStatusQuery request)
    {
        return request.Status is null ?
            CountWithoutFilter() :
            CountWithFilter(request.Status.Value);
    }

    private CommandDefinition Select(GetSubmissionsByStatusQuery request)
    {
        return request.Status is null ?
            SelectWithoutFilter(request.Pagination) :
            SelectWithFilter(request.Pagination, request.Status.Value);
    }

    private static CommandDefinition CountWithoutFilter()
    {
        return new CommandDefinition(@"SELECT COUNT(*) submissions ");
    }

    private static CommandDefinition CountWithFilter(SubmissionStatus status)
    {
        return new CommandDefinition(@"
SELECT COUNT(*) FROM submissions 
WHERE status = @status", new { status });
    }

    private static CommandDefinition SelectWithoutFilter(Pagination pagination)
    {
        return new CommandDefinition(
            @"
SELECT * FROM submissions
ORDER BY submission_date DESC
LIMIT @Limit OFFSET @Offset",
            new
            {
                pagination.Limit,
                pagination.Offset
            });
    }

    private static CommandDefinition SelectWithFilter(Pagination pagination, SubmissionStatus status)
    {
        return new CommandDefinition(
            @"
SELECT * FROM submissions
WHERE status = @status
ORDER BY submission_date DESC
LIMIT @Limit OFFSET @Offset",
            new
            {
                pagination.Limit,
                pagination.Offset,
                status
            });
    }
}
