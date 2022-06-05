using KpiV3.Domain.PeriodParts.DataContracts;
using KpiV3.Domain.PeriodParts.Queries;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.PeriodParts.Data;
using MediatR;

namespace KpiV3.Infrastructure.PeriodParts.QueryHandlers;

internal class GetPeriodPartsQueryHandler : IRequestHandler<GetPeriodPartsQuery, Result<List<PeriodPart>, IError>>
{
    private readonly Database _db;

    public GetPeriodPartsQueryHandler(Database db)
    {
        _db = db;
    }

    public async Task<Result<List<PeriodPart>, IError>> Handle(GetPeriodPartsQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
SELECT * FROM period_parts
WHERE period_id = @PeriodId";

        return await _db
            .QueryAsync<PeriodPartRow>(new(sql, new { request.PeriodId }))
            .MapAsync(rows => rows.Select(row => row.ToModel()).ToList());
    }
}
