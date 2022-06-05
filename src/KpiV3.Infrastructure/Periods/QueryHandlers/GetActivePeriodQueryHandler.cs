using KpiV3.Domain.Periods.DataContracts;
using KpiV3.Domain.Periods.Queries;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Periods.Data;
using MediatR;

namespace KpiV3.Infrastructure.Periods.QueryHandlers;

internal class GetActivePeriodQueryHandler : IRequestHandler<GetActivePeriodQuery, Result<Period, IError>>
{
    private readonly Database _db;

    public GetActivePeriodQueryHandler(Database db)
    {
        _db = db;
    }

    public async Task<Result<Period, IError>> Handle(GetActivePeriodQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
SELECT * FROM periods
WHERE from_date <= @Now AND to_date >= @Now";

        return await _db
            .QueryFirstAsync<PeriodRow>(new(sql, new { request.Now }))
            .MapAsync(row => row.ToModel());
    }
}
