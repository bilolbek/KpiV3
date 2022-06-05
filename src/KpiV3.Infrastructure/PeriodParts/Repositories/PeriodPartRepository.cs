using KpiV3.Domain.PeriodParts.DataContracts;
using KpiV3.Domain.PeriodParts.Ports;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.PeriodParts.Data;

namespace KpiV3.Infrastructure.PeriodParts.Repositories;

internal class PeriodPartRepository : IPeriodPartRepository
{

    private readonly Database _db;

    public PeriodPartRepository(Database db)
    {
        _db = db;
    }

    public async Task<Result<PeriodPart, IError>> FindByIdAsync(Guid partId)
    {
        const string sql = @"
SELECT * FROM period_parts
WHERE id = @partId";

        return await _db
            .QueryFirstAsync<PeriodPartRow>(new(sql, new { partId }))
            .MapAsync(row => row.ToModel());
    }

    public async Task<Result<IError>> InsertAsync(PeriodPart part)
    {
        const string sql = @"
INSERT INTO period_parts (id, name, from_date, to_date, period_id)
VALUES (@Id, @Name, @FromDate, @ToDate, @PeriodId)";

        return await _db.ExecuteAsync(new(sql, new PeriodPartRow(part)));
    }

    public async Task<Result<IError>> UpdateAsync(PeriodPart part)
    {
        const string sql = @"
UPDATE period_parts SET
    name = @Name,
    from_date = @FromDate,
    to_date = @ToDate,
    period_id = @PeriodId
WHERE id = @Id";

        return await _db.ExecuteRequiredChangeAsync<PeriodPart>(new(sql, new PeriodPartRow(part)));
    }

    public async Task<Result<IError>> DeleteAsync(Guid partId)
    {
        const string sql = @"
DELETE FROM period_parts
WHERE id = @partId";

        return await _db.ExecuteRequiredChangeAsync<PeriodPart>(new(sql, new { partId }));
    }

}
