using KpiV3.Domain.Specialties.DataContracts;

namespace KpiV3.WebApi.DataContracts.Specialties;

public record SpecialtyDto
{
    public SpecialtyDto(Specialty specialty)
    {
        Id = specialty.Id;
        Name = specialty.Name;
        Description = specialty.Description;
        PositionId = specialty.PositionId;
    }

    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
    public Guid PositionId { get; init; }
}
