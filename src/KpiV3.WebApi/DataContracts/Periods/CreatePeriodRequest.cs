using KpiV3.Domain.Periods.Commands;
using KpiV3.WebApi.Validation;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Periods;

public record CreatePeriodRequest
{
    [Required(AllowEmptyStrings = true)]
    public string Name { get; init; } = default!;

    public DateTimeOffset From { get; init; }

    [GreaterThan(nameof(From))]
    public DateTimeOffset To { get; init; }

    public CreatePeriodCommand ToCommand()
    {
        return new CreatePeriodCommand
        {
            Name = Name,
            Range = new()
            {
                From = From,
                To = To,
            },
        };
    }
}
