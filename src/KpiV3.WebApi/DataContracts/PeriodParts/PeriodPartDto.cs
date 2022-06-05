using KpiV3.Domain.PeriodParts.DataContracts;

namespace KpiV3.WebApi.DataContracts.PeriodParts;

public record PeriodPartDto
{
    public PeriodPartDto()
    {
    }

    public PeriodPartDto(PeriodPart part)
    {
        Id = part.Id;
        Name = part.Name;
        PeriodId = part.PeriodId;
        From = part.From;
        To = part.To;
    }

    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public Guid PeriodId { get; set; }

    public DateTimeOffset From { get; set; }
    public DateTimeOffset To { get; set; }
}
