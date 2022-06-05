using KpiV3.Domain.SpecialtyChoices.DataContracts;
using KpiV3.Domain.SpecialtyChoices.Ports;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.SpecialtyChoices.Data;

namespace KpiV3.Infrastructure.SpecialtyChoices.Repositories;

internal class SpecialtyChoiceRepository : ISpecialtyChoiceRepository
{
    private readonly Database _db;

    public SpecialtyChoiceRepository(Database db)
    {
        _db = db;
    }

    public async Task<Result<SpecialtyChoice, IError>> FindByEmployeeIdAndPeriodIdAsync(Guid employeeId, Guid periodId)
    {
        const string sql = @"
SELECT * FROM specialty_choices
WHERE employee_id = @employeeId AND period_id = @periodId";

        return await _db
            .QueryFirstAsync<SpecialtyChoiceRow>(new(sql, new { employeeId, periodId }))
            .MapAsync(row => row.ToModel());
    }

    public async Task<Result<IError>> InsertAsync(SpecialtyChoice choice)
    {
        const string sql = @"
INSERT INTO specialty_choices (employee_id, period_id, specialty_id, can_be_changed)
VALUES (@EmployeeId, @PeriodId, @SpecialtyId, @CanBeChanged)";

        return await _db.ExecuteAsync(new(sql, new SpecialtyChoiceRow(choice)));
    }

    public async Task<Result<IError>> UpdateAsync(SpecialtyChoice choice)
    {
        const string sql = @"
UPDATE specialty_choices SET
    specialty_id = @SpecialtyId,
    can_be_changed = @CanBeChanged
WHERE employee_id = @EmployeeId AND period_id = @PeriodId";

        return await _db.ExecuteRequiredChangeAsync<SpecialtyChoice>(new(sql, new SpecialtyChoiceRow(choice)));
    }
}
