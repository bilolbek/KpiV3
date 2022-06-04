using KpiV3.Domain.Periods.Commands;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Periods;

public class UpdatePeriodRequest
{
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; } = default!;
    public DateTimeOffset From { get; set; }
    public DateTimeOffset To { get; set; }

    public UpdatePeriodCommand ToCommand(Guid periodId)
    {
        return new UpdatePeriodCommand
        {
            PeriodId = periodId,
            Name = Name,
            From = From,
            To = To
        };
    }
}
