using KpiV3.Domain.Ports;

namespace KpiV3.Infrastructure.Adapters;

public class DateProvider : IDateProvider
{
    public DateTimeOffset Now()
    {
        return DateTimeOffset.Now;
    }
}
