using KpiV3.Domain.Positions.DataContracts;
using KpiV3.Domain.Positions.Ports;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Positions.Data;

namespace KpiV3.Infrastructure.Positions.Repositories;

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

    public async Task<Result<Position, IError>> FindByNameAsync(string positionName)
    {
        const string sql = @"SELECT * FROM positions WHERE name = @positionName";

        return await _db
            .QueryFirstAsync<PositionRow>(new(sql, new { positionName }))
            .MapAsync(row => row.ToModel());
    }

    public async Task<Result<IError>> InsertAsync(Position position)
    {
        const string sql = @"
INSERT INTO positions (id, type, name)
VALUES (@Id, @Type, @Name)";

        return await _db.ExecuteAsync(new(sql, new PositionRow(position)));
    }

    public async Task<Result<IError>> DeleteAsync(Guid positionId)
    {
        const string sql = @"DELETE FROM positions WHERE id = @positionId";

        return await _db.ExecuteRequiredChangeAsync<Position>(new(sql, new { positionId }));
    }

    public async Task<Result<IError>> UpdateAsync(Position position)
    {
        const string sql = @"UPDATE positions SET type = @Type, name = @Name WHERE id = @Id";

        return await _db.ExecuteRequiredChangeAsync<Position>(new(sql, new PositionRow(position)));
    }
}
