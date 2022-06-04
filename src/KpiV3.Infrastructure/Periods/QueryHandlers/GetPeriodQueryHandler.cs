using KpiV3.Domain.Periods.DataContracts;
using KpiV3.Domain.Periods.Queries;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Periods.Data;
using MediatR;

namespace KpiV3.Infrastructure.Periods.QueryHandlers;

internal class GetPeriodQueryHandler : IRequestHandler<GetPeriodQuery, Result<Period, IError>>
{
    private readonly Database _db;

    public GetPeriodQueryHandler(Database db)
    {
        _db = db;
    }

    public async Task<Result<Period, IError>> Handle(GetPeriodQuery request, CancellationToken cancellationToken)
    {
        const string sql = "SELECT * FROM periods WHERE id = @PeriodId";

        return await _db
            .QueryFirstAsync<PeriodRow>(new(sql, new { request.PeriodId }))
            .MapAsync(row => row.ToModel());
    }
}
