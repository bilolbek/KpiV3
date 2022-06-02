using KpiV3.Domain.Employees.DataContracts;

namespace KpiV3.Domain.Employees.Ports;

public interface IEmployeeRepository
{
    Task<Result<Employee, IError>> FindByEmailAsync(string email);

    Task<Result<IError>> InsertAsync(Employee employee);
}
