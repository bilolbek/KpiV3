using KpiV3.Domain.Requirements.Commands;

namespace KpiV3.WebApi.DataContracts.Requirements;

public record UpdateRequirementRequest
{
    public string? Note { get; init; }
    public double Weight { get; init; }

    public UpdateRequirementCommand ToCommand(Guid requirementId)
    {
        return new UpdateRequirementCommand
        {
            RequirementId = requirementId,
            Note = Note,
            Weight = Weight
        };
    }
}
