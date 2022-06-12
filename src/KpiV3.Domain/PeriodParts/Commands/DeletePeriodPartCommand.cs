using MediatR;

namespace KpiV3.Domain.PeriodParts.Commands;

public record DeletePeriodPartCommand : IRequest
{
    public Guid PartId { get; init; }
}

public class DeletePeriodPartCommandHandler : AsyncRequestHandler<DeletePeriodPartCommand>
{
    private readonly KpiContext _db;

    public DeletePeriodPartCommandHandler(KpiContext db)
    {
        _db = db;
    }

    protected override async Task Handle(DeletePeriodPartCommand request, CancellationToken cancellationToken)
    {
        var part = await _db.PeriodParts
            .FindAsync(new object?[] { request.PartId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        _db.PeriodParts.Remove(part);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
