using KpiV3.Domain.Specialties.DataContracts;
using KpiV3.Domain.Specialties.Ports;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Specialties.Data;

namespace KpiV3.Infrastructure.Specialties.Repositories;

internal class SpecialtyRepository : ISpecialtyRepository
{
    private readonly Database _db;

    public SpecialtyRepository(Database db)
    {
        _db = db;
    }

    public async Task<Result<IError>> InsertAsync(Specialty specialty)
    {
        const string sql = @"
INSERT INTO specialties (id, name, description, position_id)
VALUES (@Id, @Name, @Description, @PositionId)";

        return await _db.ExecuteAsync(new(sql, new SpecialtyRow(specialty)));
    }

    public async Task<Result<IError>> UpdateAsync(Specialty specialty)
    {
        const string sql = @"
UPDATE specialties SET name = @Name, description = @Description, position_id = @PositionId
WHERE id = @Id";

        return await _db.ExecuteRequiredChangeAsync<Specialty>(new(sql, new SpecialtyRow(specialty)));
    }

    public async Task<Result<IError>> DeleteAsync(Guid specialtyId)
    {
        const string sql = @"
DELETE FROM specialties
WHERE id = @specialtyId";

        return await _db.ExecuteRequiredChangeAsync<Specialty>(new(sql, new { specialtyId }));
    }

    public async Task<Result<List<Specialty>, IError>> FindByPositionIdAsync(Guid positionId)
    {
        const string sql = @"
SELECT * FROM specialties
WHERE position_id = @positionId";

        return await _db
            .QueryAsync<SpecialtyRow>(new(sql, new { positionId }))
            .MapAsync(rows => rows.Select(row => row.ToModel()).ToList());
    }
}
