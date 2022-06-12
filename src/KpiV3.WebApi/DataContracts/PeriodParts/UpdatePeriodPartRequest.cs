using KpiV3.Domain.PeriodParts.Commands;
using KpiV3.WebApi.Validation;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.PeriodParts;

public record UpdatePeriodPartRequest
{

    [Required(AllowEmptyStrings = false)]
    public string Name { get; init; } = default!;

    public DateTimeOffset From { get; init; }

    [GreaterThan(nameof(From))]
    public DateTimeOffset To { get; init; }

    public UpdatePeriodPartCommand ToCommand(Guid partId)
    {
        return new UpdatePeriodPartCommand
        {
            PartId = partId,
            Name = Name,
            Range = new()
            {
                From = From,
                To = To,
            },
        };
    }
}