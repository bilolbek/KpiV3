using KpiV3.Domain.Positions.DataContracts;

namespace KpiV3.Domain.Positions.Ports;

public interface IPositionRepository
{
    Task<Result<Position, IError>> FindByIdAsync(Guid positionId);

    Task<Result<Position, IError>> FindByNameAsync(string positionName);

    Task<Result<IError>> InsertAsync(Position position);

    Task<Result<IError>> DeleteAsync(Guid positionId);
    
    Task<Result<IError>> UpdateAsync(Position position);
}
