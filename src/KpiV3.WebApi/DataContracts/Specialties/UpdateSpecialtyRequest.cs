using KpiV3.Domain.Specialties.Commands;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Specialties;

public record UpdateSpecialtyRequest
{
    [Required(AllowEmptyStrings = false)]
    public string Name { get; init; } = default!;

    public string? Description { get; init; }

    public UpdateSpecialtyCommand ToCommand(Guid specialtyId)
    {
        return new UpdateSpecialtyCommand
        {
            SpecialtyId = specialtyId,
            Name = Name,
            Description = Description,
        };
    }
}
