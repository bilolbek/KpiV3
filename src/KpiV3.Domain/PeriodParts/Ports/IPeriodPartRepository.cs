using KpiV3.Domain.PeriodParts.DataContracts;

namespace KpiV3.Domain.PeriodParts.Ports;

public interface IPeriodPartRepository
{
    Task<Result<PeriodPart, IError>> FindByIdAsync(Guid partId);
    Task<Result<IError>> InsertAsync(PeriodPart part);
    Task<Result<IError>> UpdateAsync(PeriodPart part);
    Task<Result<IError>> DeleteAsync(Guid partId);
}
