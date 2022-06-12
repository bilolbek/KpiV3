using KpiV3.Domain.Indicators.DataContracts;
using KpiV3.Domain.PeriodParts.DataContracts;
using KpiV3.Domain.Requirements.DataContracts;
using KpiV3.Domain.Specialties.DataContracts;

namespace KpiV3.WebApi.DataContracts.Requirements;

public record RequirementDto
{
    public RequirementDto(Requirement requirement)
    {
        Id = requirement.Id;
        Note = requirement.Note;
        Weight = requirement.Weight;
        PeriodPartId = requirement.PeriodPartId;
        SpecialtyId = requirement.SpecialtyId;
        IndicatorId = requirement.IndicatorId;
    }

    public Guid Id { get; set; }
    public string? Note { get; set; }
    public double Weight { get; set; }
    public Guid PeriodPartId { get; set; }
    public Guid SpecialtyId { get; set; }
    public Guid IndicatorId { get; set; }
}
