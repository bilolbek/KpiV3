using KpiV3.Domain.Requirements.Commands;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Requirements;

public record CreateRequirementRequest
{
    public Guid SpecialtyId { get; init; }
    public Guid PeriodPartId { get; init; }
    public Guid IndicatorId { get; init; }
    public string? Note { get; init; }

    [Range(0, 100)]
    public double Weight { get; init; }

    public CreateRequirementCommand ToCommand()
    {
        return new CreateRequirementCommand
        {
            SpecialtyId = SpecialtyId,
            PeriodPartId = PeriodPartId,
            IndicatorId = IndicatorId,
            Note = Note,
            Weight = Weight,
        };
    }
}