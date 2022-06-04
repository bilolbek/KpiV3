using KpiV3.Domain.Specialties.DataContracts;

namespace KpiV3.WebApi.DataContracts.Specialties;

public class SpecialtyDto
{
    public SpecialtyDto()
    {
    }

    public SpecialtyDto(Specialty specialty)
    {
        Id = specialty.Id;
        Name = specialty.Name;
        Description = specialty.Description;
        PositionId = specialty.PositionId;
    }

    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public Guid PositionId { get; set; }
}
