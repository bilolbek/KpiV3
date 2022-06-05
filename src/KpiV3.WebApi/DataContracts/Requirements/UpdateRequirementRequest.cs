using KpiV3.Domain.Requirements.Commands;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Requirements;

public record UpdateRequirementRequest
{
    public string? Note { get; set; }

    [Range(0, double.MaxValue)]
    public double Weight { get; set; }

    public UpdateRequirementCommand ToCommand(Guid requirementId)
    {
        return new UpdateRequirementCommand
        {
            RequirementId = requirementId,
            Note = Note,
            Weight = Weight,
        };
    }
}
