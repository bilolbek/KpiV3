using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Ports;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Employees.Data;

namespace KpiV3.Infrastructure.Employees.Repositories;

internal class PositionRepository : IPositionRepository
{
    private readonly Database _db;

    public PositionRepository(Database db)
    {
        _db = db;
    }

    public async Task<Result<IError>> InsertAsync(Position position)
    {
        const string sql = @"
INSERT INTO positions (id, name)
VALUES (@Id, @Name)";

        return await _db.ExecuteAsync(new(sql, new PositionRow(position)));
    }
}
