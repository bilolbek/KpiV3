using KpiV3.Domain.Requirements.Commands;

namespace KpiV3.WebApi.DataContracts.Requirements;

public record CreateRequirementRequest
{
    public Guid PeriodPartId { get; set; }
    public Guid SpecialtyId { get; set; }
    public Guid IndicatorId { get; set; }
    public string? Note { get; set; }
    public double Weight { get; set; }

    public CreateRequirementCommand ToCommand()
    {
        return new CreateRequirementCommand
        {
            PeriodPartId = PeriodPartId,
            SpecialtyId = SpecialtyId,
            IndicatorId = IndicatorId,
            Note = Note,
            Weight = Weight
        };
    }
}
