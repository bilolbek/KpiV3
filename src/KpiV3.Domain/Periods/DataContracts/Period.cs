namespace KpiV3.Domain.Periods.DataContracts;

public record Period
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public DateTimeOffset From { get; set; }
    public DateTimeOffset To { get; set; }
}