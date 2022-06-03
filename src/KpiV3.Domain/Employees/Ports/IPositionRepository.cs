using KpiV3.Domain.Employees.DataContracts;

namespace KpiV3.Domain.Employees.Ports;

public interface IPositionRepository
{
    Task<Result<Position, IError>> FindByIdAsync(Guid positionId);

    Task<Result<Position, IError>> FindByNameAsync(string positionName);

    Task<Result<IError>> InsertAsync(Position position);
}
