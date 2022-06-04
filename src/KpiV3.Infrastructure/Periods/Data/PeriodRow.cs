using KpiV3.Domain.Periods.DataContracts;

namespace KpiV3.Infrastructure.Periods.Data;

internal class PeriodRow
{
    public PeriodRow()
    {
    }

    public PeriodRow(Period period)
    {
        Id = period.Id;
        Name = period.Name;
        FromDate = period.From;
        ToDate = period.To;
    }

    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public DateTimeOffset FromDate { get; set; }
    public DateTimeOffset ToDate { get; set; }

    public Period ToModel()
    {
        return new Period 
        { 
            Id = Id,
            Name = Name,
            From = FromDate,
            To = ToDate,
        };
    }
}
