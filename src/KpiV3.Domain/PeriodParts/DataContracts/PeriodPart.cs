using KpiV3.Domain.Common.DataContracts;
using KpiV3.Domain.Periods.DataContracts;

namespace KpiV3.Domain.PeriodParts.DataContracts;


public class PeriodPart
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public DateRange Range { get; set; } = default!;

    public Guid PeriodId { get; set; }
    public Period Period { get; set; } = default!;

    public bool IsActive(DateTimeOffset date)
    {
        return Range.Includes(date);
    }
}
