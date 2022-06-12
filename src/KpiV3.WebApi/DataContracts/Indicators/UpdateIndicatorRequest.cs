using KpiV3.Domain.Indicators.Commands;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Indicators;

public record UpdateIndicatorRequest
{
    [Required(AllowEmptyStrings = false)]
    public string Name { get; init; } = default!;
    [Required(AllowEmptyStrings = false)]
    public string Description { get; init; } = default!;
    public string? Comment { get; init; }

    public UpdateIndicatorCommand ToCommand(Guid indicatorId)
    {
        return new UpdateIndicatorCommand
        {
            IndicatorId = indicatorId,
            Name = Name,
            Description = Description,
            Comment = Comment,
        };
    }
}
