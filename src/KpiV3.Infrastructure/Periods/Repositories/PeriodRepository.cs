using KpiV3.Domain.Periods.DataContracts;
using KpiV3.Domain.Periods.Ports;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Periods.Data;

namespace KpiV3.Infrastructure.Periods.Repositories;

internal class PeriodRepository : IPeriodRepository
{
    private readonly Database _db;

    public PeriodRepository(Database db)
    {
        _db = db;
    }

    public async Task<Result<IError>> InsertAsync(Period period)
    {
        const string sql = @"
INSERT INTO periods (id, name, from_date, to_date)
VALUES (@Id, @Name, @FromDate, @ToDate)";

        return await _db.ExecuteAsync(new(sql, new PeriodRow(period)));
    }

    public async Task<Result<IError>> UpdateAsync(Period period)
    {
        const string sql = @"
UPDATE periods SET name = @Name, from_date = @FromDate, to_date = @ToDate
WHERE id = @Id";

        return await _db.ExecuteAsync(new(sql, new PeriodRow(period)));
    }

    public async Task<Result<IError>> DeleteAsync(Guid periodId)
    {
        const string sql = @"DELETE FROM periods WHERE id = @periodId";

        return await _db.ExecuteRequiredChangeAsync<Period>(new(sql, new { periodId }));
    }
}
