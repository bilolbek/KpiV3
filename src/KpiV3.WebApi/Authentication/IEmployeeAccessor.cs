using KpiV3.Domain.Employees.DataContracts;

namespace KpiV3.WebApi.Authentication;

public interface IEmployeeAccessor
{
    Guid EmployeeId { get; }
    Position Position { get; }
}
