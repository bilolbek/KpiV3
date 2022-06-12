using KpiV3.Domain.Periods.DataContracts;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Periods;

public record PeriodDto
{
    public PeriodDto(Period period)
    {
        Id = period.Id;
        Name = period.Name;

    }

    public Guid Id { get; init; }

    public string Name { get; init; } = default!;
    
    public DateTimeOffset From { get; init; }
    
    public DateTimeOffset To { get; init; }
}
