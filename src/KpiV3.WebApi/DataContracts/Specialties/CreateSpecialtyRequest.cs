using KpiV3.Domain.Specialties.Commands;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Specialties;

public record CreateSpecialtyRequest
{
    public Guid PositionId { get; init; }

    [Required(AllowEmptyStrings = false)]
    public string Name { get; init; } = default!;
    
    public string? Description { get; init; }

    public CreateSpecialtyCommand ToCommand()
    {
        return new CreateSpecialtyCommand
        {
            PositionId = PositionId,
            Name = Name,
            Description = Description,
        };
    }
}