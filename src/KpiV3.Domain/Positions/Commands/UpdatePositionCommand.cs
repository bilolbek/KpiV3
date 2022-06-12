using KpiV3.Domain.Positions.DataContracts;
using MediatR;

namespace KpiV3.Domain.Positions.Commands;

public record UpdatePositionCommand : IRequest<Position>
{
    public Guid PositionId { get; init; }
    public string Name { get; init; } = default!;
}

public class UpdatePositionCommandHandler : IRequestHandler<UpdatePositionCommand, Position>
{
    private readonly KpiContext _db;

    public UpdatePositionCommandHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<Position> Handle(UpdatePositionCommand request, CancellationToken cancellationToken)
    {
        var position = await _db.Positions
            .FindAsync(new object?[] { request.PositionId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        position.Name = request.Name;

        await _db.SaveChangesAsync(cancellationToken);

        return position;
    }
}
