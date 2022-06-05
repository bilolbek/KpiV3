using KpiV3.Domain.Requirements.DataContracts;

namespace KpiV3.WebApi.DataContracts.Requirements;

public record RequirementDto
{
    public RequirementDto()
    {
    }

    public RequirementDto(Requirement requirement)
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
}
