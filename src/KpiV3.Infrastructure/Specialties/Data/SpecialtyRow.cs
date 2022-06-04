using KpiV3.Domain.Specialties.DataContracts;

namespace KpiV3.Infrastructure.Specialties.Data;

internal class SpecialtyRow
{
    public SpecialtyRow()
    {
    }

    public SpecialtyRow(Specialty specialty)
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

    public Specialty ToModel()
    {
        return new Specialty
        {
            Id = Id,
            Name = Name,
            Description = Description,
            PositionId = PositionId,
        };
    }
}
