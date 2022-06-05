namespace KpiV3.Domain.PeriodParts.DataContracts;

public record PeriodPart
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = default!;

    public Guid PeriodId { get; set; }

    public DateTimeOffset From { get; set; }
    public DateTimeOffset To { get; set; }
}
