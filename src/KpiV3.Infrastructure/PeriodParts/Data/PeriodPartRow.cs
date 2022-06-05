using KpiV3.Domain.PeriodParts.DataContracts;

namespace KpiV3.Infrastructure.PeriodParts.Data;

internal class PeriodPartRow
{
    public PeriodPartRow()
    {
    }

    public PeriodPartRow(PeriodPart part)
    {
        Id = part.Id;
        Name = part.Name;
        PeriodId = part.PeriodId;
        FromDate = part.From;
        ToDate = part.To;
    }

    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public Guid PeriodId { get; set; }
    public DateTimeOffset FromDate { get; set; }
    public DateTimeOffset ToDate { get; set; }

    public PeriodPart ToModel()
    {
        return new PeriodPart
        {
            Id = Id,
            Name = Name,
            PeriodId = PeriodId,
            From = FromDate,
            To = ToDate,
        };
    }
}
