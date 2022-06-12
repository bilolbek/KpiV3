using KpiV3.Domain.Common.DataContracts;
using KpiV3.Domain.PeriodParts.DataContracts;

namespace KpiV3.Domain.Periods.DataContracts;

public class Period
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public DateRange Range { get; set; } = default!;

    public IEnumerable<PeriodPart> PeriodParts { get; set; } = default!;
    
    public bool IsActive(DateTimeOffset date)
    {
        return Range.Includes(date);
    }

    public bool Includes(DateRange range)
    {
        return Range.Includes(range);
    }
}
