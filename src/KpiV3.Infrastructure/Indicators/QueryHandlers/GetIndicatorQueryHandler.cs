using KpiV3.Domain.Indicators.DataContracts;
using KpiV3.Domain.Indicators.Queries;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Indicators.Data;
using MediatR;

namespace KpiV3.Infrastructure.Indicators.QueryHandlers;

internal class GetIndicatorQueryHandler : IRequestHandler<GetIndicatorQuery, Result<Indicator, IError>>
{
    private readonly Database _db;

    public GetIndicatorQueryHandler(Database db)
    {
        _db = db;
    }

    public async Task<Result<Indicator, IError>> Handle(GetIndicatorQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
SELECT * FROM indicators
WHERE id = @IndicatorId";

        return await _db
            .QueryFirstAsync<IndicatorRow>(new(sql, new { request.IndicatorId }))
            .MapAsync(row => row.ToModel());
    }
}
