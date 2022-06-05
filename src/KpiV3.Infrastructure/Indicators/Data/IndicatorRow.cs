using KpiV3.Domain.Indicators.DataContracts;

namespace KpiV3.Infrastructure.Indicators.Data;

internal class IndicatorRow
{
    public IndicatorRow()
    {
    }

    public IndicatorRow(Indicator indicator)
    {
        Id = indicator.Id;
        Name = indicator.Name;
        Description = indicator.Description;
        Comment = indicator.Comment;
    }

    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Comment { get; set; } = default!;

    public Indicator ToModel()
    {
        return new Indicator
        {
            Id = Id,
            Name = Name,
            Description = Description,
            Comment = Comment,
        };
    }
}
