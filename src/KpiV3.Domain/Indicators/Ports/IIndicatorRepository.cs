using KpiV3.Domain.Indicators.DataContracts;

namespace KpiV3.Domain.Indicators.Ports;

public interface IIndicatorRepository
{
    Task<Result<IError>> InsertAsync(Indicator indicator);
    Task<Result<IError>> UpdateAsync(Indicator indicator);
    Task<Result<IError>> DeleteAsync(Guid indicatorId);
}
