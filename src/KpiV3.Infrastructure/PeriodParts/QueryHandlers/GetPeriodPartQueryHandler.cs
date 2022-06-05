using KpiV3.Domain.PeriodParts.DataContracts;
using KpiV3.Domain.PeriodParts.Queries;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.PeriodParts.Data;
using MediatR;

namespace KpiV3.Infrastructure.PeriodParts.QueryHandlers;

internal class GetPeriodPartQueryHandler : IRequestHandler<GetPeriodPartQuery, Result<PeriodPart, IError>>
{
    private readonly Database _db;

    public GetPeriodPartQueryHandler(Database db)
    {
        _db = db;
    }

    public async Task<Result<PeriodPart, IError>> Handle(GetPeriodPartQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
SELECT * FROM period_parts
WHERE id = @PartId";

        return await _db
            .QueryFirstAsync<PeriodPartRow>(new(sql, new { request.PartId }))
            .MapAsync(row => row.ToModel());
    }
}
