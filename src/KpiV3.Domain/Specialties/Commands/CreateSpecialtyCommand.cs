using KpiV3.Domain.Common.Ports;
using KpiV3.Domain.Specialties.DataContracts;
using MediatR;

namespace KpiV3.Domain.Specialties.Commands;

public record CreateSpecialtyCommand : IRequest<Specialty>
{
    public Guid PositionId { get; init; }
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
}

public class CreateSpecialtyCommandHandler : IRequestHandler<CreateSpecialtyCommand, Specialty>
{
    private readonly KpiContext _db;
    private readonly IGuidProvider _guidProvider;

    public CreateSpecialtyCommandHandler(
        KpiContext db,
        IGuidProvider guidProvider)
    {
        _db = db;
        _guidProvider = guidProvider;
    }

    public async Task<Specialty> Handle(CreateSpecialtyCommand request, CancellationToken cancellationToken)
    {
        var specialty = new Specialty
        {
            Id = _guidProvider.New(),
            Name = request.Name,
            Description = request.Description,
            PositionId = request.PositionId,
        };

        _db.Specialties.Add(specialty);

        await _db.SaveChangesAsync(cancellationToken);

        return specialty;
    }
}