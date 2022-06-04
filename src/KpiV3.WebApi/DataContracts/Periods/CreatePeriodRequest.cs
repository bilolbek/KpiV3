using KpiV3.Domain.Periods.Commands;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Periods;

public record CreatePeriodRequest
{
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; } = default!;
    public DateTimeOffset From { get; set; }
    public DateTimeOffset To { get; set; }

    public CreatePeriodCommand ToCommand()
    {
        return new CreatePeriodCommand
        {
            Name = Name,
            From = From,
            To = To
        };
    }
}
