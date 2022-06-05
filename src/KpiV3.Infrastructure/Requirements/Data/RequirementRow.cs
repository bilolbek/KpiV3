using KpiV3.Domain.Requirements.DataContracts;

namespace KpiV3.Infrastructure.Requirements.Data;

internal class RequirementRow
{
    public RequirementRow()
    {
    }

    public RequirementRow(Requirement requirement)
    {
        Id = requirement.Id;
        PeriodPartId = requirement.PeriodPartId;
        SpecialtyId = requirement.SpecialtyId;
        IndicatorId = requirement.IndicatorId;
        Weight = requirement.Weight;
        Note = requirement.Note;
    }

    public Guid Id { get; set; }
    public Guid PeriodPartId { get; set; }
    public Guid SpecialtyId { get; set; }
    public Guid IndicatorId { get; set; }
    public double Weight { get; set; }
    public string? Note { get; set; }

    public Requirement ToModel()
    {
        return new Requirement
        {
            Id = Id,
            PeriodPartId = PeriodPartId,
            SpecialtyId = SpecialtyId,
            IndicatorId = IndicatorId,
            Weight = Weight,
            Note = Note
        };
    }
}
