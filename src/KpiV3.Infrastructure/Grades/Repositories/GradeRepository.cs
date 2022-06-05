using KpiV3.Domain.Grades.DataContracts;
using KpiV3.Domain.Grades.Ports;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Grades.Data;

namespace KpiV3.Infrastructure.Grades.Repositories;

internal class GradeRepository : IGradeRepository
{
    private readonly Database _db;

    public GradeRepository(Database db)
    {
        _db = db;
    }

    public async Task<Result<IError>> InsertAsync(Grade grade)
    {
        const string sql = @"
INSERT INTO grades (employee_id, requirement_id, value)
VALUES (@EmployeeId, @RequirementId, @Value)";

        return await _db.ExecuteAsync(new(sql, new GradeRow(grade)));
    }

    public async Task<Result<IError>> UpdateAsync(Grade grade)
    {
        const string sql = @"
UPDATE grades SET
    value = @Value
WHERE employee_id = @EmployeeId AND requirement_id = @RequirementId";

        return await _db.ExecuteRequiredChangeAsync<Grade>(new(sql, new GradeRow(grade)));
    }


    public async Task<Result<IError>> DeleteAsync(Guid employeeId, Guid requirementId)
    {
        const string sql = @"
DELETE FROM grades
WHERE employee_id = @employeeId AND requirement_id = @requirementId";

        return await _db.ExecuteRequiredChangeAsync<Grade>(new(sql, new
        {
            employeeId,
            requirementId
        }));
    }
}
