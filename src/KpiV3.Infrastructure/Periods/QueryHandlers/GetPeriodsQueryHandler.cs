using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Periods.DataContracts;
using KpiV3.Domain.Periods.Queries;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Periods.Data;
using MediatR;

namespace KpiV3.Infrastructure.Periods.QueryHandlers;

internal class GetPeriodsQueryHandler : IRequestHandler<GetPeriodsQuery, Result<Page<Period>, IError>>
{
    private readonly Database _db;

    public GetPeriodsQueryHandler(Database db)
    {
        _db = db;
    }

    public async Task<Result<Page<Period>, IError>> Handle(GetPeriodsQuery request, CancellationToken cancellationToken)
    {
        const string count = @"
SELECT COUNT(*) FROM periods";

        const string select = @"
SELECT * FROM periods
ORDER BY from_date DESC
LIMIT @limit OFFSET @offset";

        return await _db
            .QueryFirstAsync<int>(new(count))
            .BindAsync(total => _db
                .QueryAsync<PeriodRow>(new(select, new
                {
                    request.Pagination.Limit,
                    request.Pagination.Offset,
                }))
                .MapAsync(rows => new Page<PeriodRow>(total, request.Pagination, rows)))
            .MapAsync(rows => rows.Map(row => row.ToModel()));
    }
}
