namespace KpiV3.Domain.Indicators.DataContracts;

public record Indicator
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Comment { get; set; } = default!;
}
