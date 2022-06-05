using KpiV3.Domain.Requirements.DataContracts;
using KpiV3.Domain.Requirements.Ports;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Requirements.Data;

namespace KpiV3.Infrastructure.Requirements.Repositories;

internal class RequirementRepository : IRequirementRepository
{
    private readonly Database _db;

    public RequirementRepository(Database db)
    {
        _db = db;
    }


    public async Task<Result<Requirement, IError>> FindByIdAsync(Guid requirementId)
    {
        const string sql = @"SELECT * FROM requirements WHERE id = @requirementId";

        return await _db
            .QueryFirstAsync<RequirementRow>(new(sql, new { requirementId }))
            .MapAsync(row => row.ToModel());
    }

    public async Task<Result<List<Requirement>, IError>> FindBySpecialtyIdAndPeriodPartIdAsync(Guid specialtyId, Guid periodPartId)
    {
        const string sql = @"
SELECT * FROM requirements
WHERE specialty_id = @specialtyId AND period_part_id = @periodPartId";

        return await _db
            .QueryAsync<RequirementRow>(new(sql, new { specialtyId, periodPartId }))
            .MapAsync(rows => rows.Select(row => row.ToModel()).ToList());
    }

    public async Task<Result<IError>> InsertAsync(Requirement requirement)
    {
        const string sql = @"
INSERT INTO requirements (id, specialty_id, period_part_id, indicator_id, note, weight)
VALUES (@Id, @SpecialtyId, @PeriodPartId, @IndicatorId, @Note, @Weight)";

        return await _db.ExecuteAsync(new(sql, new RequirementRow(requirement)));
    }

    public async Task<Result<IError>> UpdateAsync(Requirement requirement)
    {
        const string sql = @"
UPDATE requirements SET 
    specialty_id = @SpecialtyId,
    period_part_id = @PeriodPartId,
    indicator_id = @IndicatorId,
    note = @Note,
    weight = @Weight
WHERE id = @Id";

        return await _db.ExecuteRequiredChangeAsync<Requirement>(new(sql, new RequirementRow(requirement)));
    }

    public async Task<Result<IError>> DeleteAsync(Guid requirementId)
    {
        const string sql = @"
DELETE FROM requirements
WHERE id = @requirementId";

        return await _db.ExecuteRequiredChangeAsync<Requirement>(new(sql, new { requirementId }));
    }
}
