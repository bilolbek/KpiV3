using KpiV3.Domain.Indicators.DataContracts;

namespace KpiV3.WebApi.DataContracts.Indicators;

public class IndicatorDto
{
    public IndicatorDto()
    {
    }

    public IndicatorDto(Indicator indicator)
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
}
