using MediatR;

namespace KpiV3.Domain.Positions.Commands;

public record DeletePositionCommand : IRequest
{
    public Guid PositionId { get; init; }
}

public class DeletePositionCommandHandler : AsyncRequestHandler<DeletePositionCommand>
{
    private readonly KpiContext _db;

    public DeletePositionCommandHandler(KpiContext db)
    {
        _db = db;
    }

    protected override async Task Handle(DeletePositionCommand request, CancellationToken cancellationToken)
    {
        var position = await _db.Positions
            .FindAsync(new object?[] { request.PositionId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        if (position.Type is DataContracts.PositionType.Root)
        {
            throw new BusinessLogicException("It's not allowed to delete root positions");
        }

        _db.Positions.Remove(position);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
