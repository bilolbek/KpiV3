using KpiV3.Domain.Common.DataContracts;
using KpiV3.Domain.PeriodParts.DataContracts;
using KpiV3.WebApi.Validation;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.PeriodParts;

public record PeriodPartDto
{
    public PeriodPartDto(PeriodPart part)
    {
        Id = part.Id;
        Name = part.Name;
        From = part.Range.From;
        To = part.Range.To;
        PeriodId = part.PeriodId;
    }

    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public DateTimeOffset From { get; init; }
    public DateTimeOffset To { get; init; }
    public Guid PeriodId { get; init; }
}
