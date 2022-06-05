using KpiV3.Domain.Indicators.Commands;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Indicators;

public record CreateIndicatorRequest
{
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; } = default!;

    [Required(AllowEmptyStrings = false)]
    public string Description { get; set; } = default!;

    [Required(AllowEmptyStrings = false)]
    public string Comment { get; set; } = default!;

    public CreateIndicatorCommand ToCommand()
    {
        return new CreateIndicatorCommand
        {
            Name = Name,
            Description = Description,
            Comment = Comment,
        };
    }
}
