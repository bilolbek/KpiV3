using KpiV3.Domain.Specialties.Commands;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Specialties;

public record CreateSpecialtyRequest
{
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; } = default!;

    [Required(AllowEmptyStrings = false)]
    public string Description { get; set; } = default!;

    public Guid PositionId { get; set; }

    public CreateSpecialtyCommand ToCommand()
    {
        return new CreateSpecialtyCommand
        {
            Name = Name,
            Description = Description,
            PositionId = PositionId,
        };
    }
}
