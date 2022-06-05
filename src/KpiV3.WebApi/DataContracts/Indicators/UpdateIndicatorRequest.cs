using KpiV3.Domain.Indicators.Commands;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Indicators;

public record UpdateIndicatorRequest
{
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; } = default!;

    [Required(AllowEmptyStrings = false)]
    public string Description { get; set; } = default!;

    [Required(AllowEmptyStrings = false)]
    public string Comment { get; set; } = default!;

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
