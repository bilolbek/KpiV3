using KpiV3.Domain.Specialties.Commands;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Specialties;

public record UpdateSpecialtyRequest
{
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; } = default!;

    [Required(AllowEmptyStrings = false)]
    public string Description { get; set; } = default!;

    public Guid PositionId { get; set; }

    public UpdateSpecialtyCommand ToCommand(Guid specialtyId)
    {
        return new UpdateSpecialtyCommand
        {
            SpecialtyId = specialtyId,
            Name = Name,
            Description = Description,
            PositionId = PositionId,
        };
    }
}
