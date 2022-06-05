using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Indicators.DataContracts;
using KpiV3.Domain.Indicators.Queries;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Indicators.Data;
using MediatR;

namespace KpiV3.Infrastructure.Indicators.QueryHandlers;

internal class GetIndicatorsQueryHandler : IRequestHandler<GetIndicatorsQuery, Result<Page<Indicator>, IError>>
{
    private readonly Database _db;

    public GetIndicatorsQueryHandler(Database db)
    {
        _db = db;
    }

    public async Task<Result<Page<Indicator>, IError>> Handle(GetIndicatorsQuery request, CancellationToken cancellationToken)
    {
        const string count = @"
SELECT COUNT(*) FROM indicators";

        const string select = @"
SELECT * FROM indicators
ORDER BY name
LIMIT @Limit OFFSET @Offset";

        return await _db
            .QueryFirstAsync<int>(new(count))
            .BindAsync(total => _db.QueryAsync<IndicatorRow>(new(select, new
            {
                request.Pagination.Limit,
                request.Pagination.Offset,
            })).MapAsync(rows => new Page<IndicatorRow>(total, request.Pagination, rows)))
            .MapAsync(rows => rows.Map(row => row.ToModel()));
    }
}
