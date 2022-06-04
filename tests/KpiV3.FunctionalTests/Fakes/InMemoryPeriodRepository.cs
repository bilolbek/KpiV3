using KpiV3.Domain.Periods.DataContracts;
using KpiV3.Domain.Periods.Ports;

namespace KpiV3.FunctionalTests.Fakes;

public class InMemoryPeriodRepository : IPeriodRepository
{
    public Dictionary<Guid, Period> Items { get; } = new();

    public Task<Result<IError>> InsertAsync(Period period)
    {
        Items[period.Id] = period;

        return Task.FromResult(Result<IError>.Ok());
    }

    public Task<Result<IError>> UpdateAsync(Period period)
    {
        Items[period.Id] = period;

        return Task.FromResult(Result<IError>.Ok());
    }

    public Task<Result<IError>> DeleteAsync(Guid periodId)
    {
        Items.Remove(periodId);

        return Task.FromResult(Result<IError>.Ok());
    }
}
