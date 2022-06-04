using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Ports;

namespace KpiV3.FunctionalTests.Fakes;

public class InMemoryEmployeeRepository : IEmployeeRepository
{
    public InMemoryEmployeeRepository()
    {
        Items = new();
    }

    public Dictionary<Guid, Employee> Items { get; }

    public Task<Result<Employee, IError>> FindByEmailAsync(string email)
    {
        if (Items.Values.FirstOrDefault(e => e.Email == email) is { } employee)
        {
            return Task.FromResult(Result<Employee, IError>.Ok(employee));
        }

        return Task.FromResult(Result<Employee, IError>.Fail(new NoEntity(typeof(Employee))));
    }

    public Task<Result<IError>> InsertAsync(Employee employee)
    {
        Items[employee.Id] = employee;
        return Task.FromResult(Result<IError>.Ok());
    }
}
