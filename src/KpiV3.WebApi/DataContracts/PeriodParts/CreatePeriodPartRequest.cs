using KpiV3.Domain.PeriodParts.Commands;
using KpiV3.WebApi.Validation;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.PeriodParts;

public record CreatePeriodPartRequest
{
    public Guid PeriodId { get; init; }

    [Required(AllowEmptyStrings = false)]
    public string Name { get; init; } = default!;

    public DateTimeOffset From { get; init; }

    [GreaterThan(nameof(From))]
    public DateTimeOffset To { get; init; }

    public CreatePeriodPartCommand ToCommand()
    {
        return new CreatePeriodPartCommand
        {
            PeriodId = PeriodId,
            Name = Name,
            Range = new()
            {
                From = From,
                To = To,
            },
        };
    }
}
