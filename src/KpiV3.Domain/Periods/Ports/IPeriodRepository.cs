using KpiV3.Domain.Periods.DataContracts;

namespace KpiV3.Domain.Periods.Ports;

public interface IPeriodRepository
{
    Task<Result<IError>> InsertAsync(Period period);
    Task<Result<IError>> UpdateAsync(Period period);
    Task<Result<IError>> DeleteAsync(Guid periodId);
}
