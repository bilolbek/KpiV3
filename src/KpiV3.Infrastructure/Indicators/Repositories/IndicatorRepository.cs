using KpiV3.Domain.Indicators.DataContracts;
using KpiV3.Domain.Indicators.Ports;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Indicators.Data;

namespace KpiV3.Infrastructure.Indicators.Repositories;

internal class IndicatorRepository : IIndicatorRepository
{
    private readonly Database _db;

    public IndicatorRepository(Database db)
    {
        _db = db;
    }

    public async Task<Result<IError>> InsertAsync(Indicator indicator)
    {
        const string sql = @"
INSERT INTO indicators (id, name, description, comment)
VALUES (@Id, @Name, @Description, @Comment)";

        return await _db.ExecuteAsync(new(sql, new IndicatorRow(indicator)));
    }

    public async Task<Result<IError>> UpdateAsync(Indicator indicator)
    {
        const string sql = @"
UPDATE indicators SET name = @Name, description = @Description, comment = @Comment
WHERE id = @Id";

        return await _db.ExecuteRequiredChangeAsync<Indicator>(new(sql, new IndicatorRow(indicator)));
    }

    public async Task<Result<IError>> DeleteAsync(Guid indicatorId)
    {
        const string sql = @"
DELETE FROM indicators 
WHERE id = @indicatorId";

        return await _db.ExecuteRequiredChangeAsync<Indicator>(new(sql, new { indicatorId }));
    }
}
