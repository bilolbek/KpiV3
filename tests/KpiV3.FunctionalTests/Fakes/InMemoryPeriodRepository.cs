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

    public Task<Result<Period, IError>> FindByIdAsync(Guid periodId)
    {
        if (Items.TryGetValue(periodId, out var period))
        {
            return Task.FromResult(Result<Period, IError>.Ok(period));
        }

        return Task.FromResult(Result<Period, IError>.Fail(new NoEntity(typeof(Period))));
    }
}
