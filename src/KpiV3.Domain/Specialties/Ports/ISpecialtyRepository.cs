using KpiV3.Domain.Specialties.DataContracts;

namespace KpiV3.Domain.Specialties.Ports;

public interface ISpecialtyRepository
{
    Task<Result<List<Specialty>, IError>> FindByPositionIdAsync(Guid positionId);
    Task<Result<IError>> InsertAsync(Specialty specialty);
    Task<Result<IError>> UpdateAsync(Specialty specialty);
    Task<Result<IError>> DeleteAsync(Guid specialtyId);
}
