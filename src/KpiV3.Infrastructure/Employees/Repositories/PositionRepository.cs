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

    public async Task<Result<Position, IError>> FindByIdAsync(Guid positionId)
    {
        const string sql = @"SELECT * FROM positions WHERE id = @positionId";

        return await _db
            .QueryFirstAsync<PositionRow>(new(sql, new { positionId }))
            .MapAsync(row => row.ToModel());
    }

    public async Task<Result<IError>> InsertAsync(Position position)
    {
        const string sql = @"
INSERT INTO positions (id, name)
VALUES (@Id, @Name)";

        return await _db.ExecuteAsync(new(sql, new PositionRow(position)));
    }
}
