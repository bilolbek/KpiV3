using KpiV3.Domain.Employees.DataContracts;

namespace KpiV3.Domain.Employees.Ports;

public interface IPositionRepository
{
    Task<Result<IError>> InsertAsync(Position position);
}
