using KpiV3.Domain.Indicators.Commands;

namespace KpiV3.WebApi.DataContracts.Indicators;

public record CreateIndicatorRequest
{
    public string Name { get; set; } = default!;
    public string Description { get; init; } = default!;
    public string? Comment { get; init; }

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
