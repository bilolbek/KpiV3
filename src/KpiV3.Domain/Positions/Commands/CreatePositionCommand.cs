using KpiV3.Domain.Positions.DataContracts;
using KpiV3.Domain.Specialties.DataContracts;
using MediatR;

namespace KpiV3.Domain.Positions.Commands;

public record CreatePositionCommand : IRequest<Position>
{
    public string Name { get; init; } = default!;
    public PositionType Type { get; init; }
}

public class CreatePositionCommandHandler : IRequestHandler<CreatePositionCommand, Position>
{
    private readonly KpiContext _db;
    private readonly IGuidProvider _guidProvider;

    public CreatePositionCommandHandler(
        KpiContext db,
        IGuidProvider guidProvider)
    {
        _db = db;
        _guidProvider = guidProvider;
    }

    public async Task<Position> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
    {
        var position = new Position
        {
            Id = _guidProvider.New(),
            Name = request.Name,
            Type = request.Type,
            Specialties = new List<Specialty>(),
        };

        _db.Positions.Add(position);

        await _db.SaveChangesAsync(cancellationToken);

        return position;
    }
}
