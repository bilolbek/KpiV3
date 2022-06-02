using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Ports;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Employees.Data;

namespace KpiV3.Infrastructure.Employees.Repositories;

internal class EmployeeRepository : IEmployeeRepository
{
    private readonly Database _db;

    public EmployeeRepository(Database db)
    {
        _db = db;
    }

    public async Task<Result<IError>> InsertAsync(Employee employee)
    {
        const string sql = @"
INSERT INTO employees(id, email, first_name, last_name, middle_name, position_id, reg_date)
VALUES (@Id, @Email, @FirstName, @LastName, @MiddleName, @PositionId, @RegDate)";

        return await _db.ExecuteAsync(new(sql, new EmployeeRow(employee)));
    }
}
