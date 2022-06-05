using KpiV3.Domain.PeriodParts.Commands;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.PeriodParts;

public record UpdatePeriodPartRequest
{
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; } = default!;
    public DateTimeOffset From { get; set; }
    public DateTimeOffset To { get; set; }

    public UpdatePeriodPartCommand ToCommand(Guid partId)
    {
        return new UpdatePeriodPartCommand
        {
            PartId = partId,
            Name = Name,
            From = From,
            To = To,
        };
    }
}
