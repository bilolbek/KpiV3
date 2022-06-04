using KpiV3.Domain.Periods.DataContracts;

namespace KpiV3.WebApi.DataContracts.Periods;

public record PeriodDto
{
    public PeriodDto()
    {
    }

    public PeriodDto(Period period)
    {
        Id = period.Id;
        Name = period.Name;
        From = period.From;
        To = period.To;
    }

    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public DateTimeOffset From { get; set; }
    public DateTimeOffset To { get; set; }
}
