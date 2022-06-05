using KpiV3.Domain.Requirements.DataContracts;
using KpiV3.Domain.Requirements.Queries;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Requirements.Data;
using MediatR;

namespace KpiV3.Infrastructure.Requirements.QueryHandlers;

internal class GetRequirementsQueryHandler : IRequestHandler<GetRequirementsQuery, Result<List<Requirement>, IError>>
{
    private readonly Database _db;

    public GetRequirementsQueryHandler(Database db)
    {
        _db = db;
    }

    public async Task<Result<List<Requirement>, IError>> Handle(GetRequirementsQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
SELECT r.* FROM requirements r
INNER JOIN period_parts pr on pr.id = r.period_part_id
WHERE specialty_id = @SpecialtyId AND pr.period_id = @PeriodId";

        return await _db
            .QueryAsync<RequirementRow>(new(sql, new { request.PeriodId, request.SpecialtyId }))
            .MapAsync(rows => rows.Select(r => r.ToModel()).ToList());
    }
}
