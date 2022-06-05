using KpiV3.Domain.PeriodParts.Commands;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.PeriodParts;

public record CreatePeriodPartRequest
{
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; } = default!;
    public Guid PeriodId { get; set; }
    public DateTimeOffset From { get; set; }
    public DateTimeOffset To { get; set; }

    public CreatePeriodPartCommand ToCommand()
    {
        return new CreatePeriodPartCommand
        {
            From = From,
            To = To,
            Name = Name,
            PeriodId = PeriodId,
        };
    }
}
