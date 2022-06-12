using MediatR;

namespace KpiV3.Domain.Indicators.Commands;

public record DeleteIndicatorCommand : IRequest
{
    public Guid IndicatorId { get; init; }
}

public class DeleteIndicatorCommandHandler : AsyncRequestHandler<DeleteIndicatorCommand>
{
    private readonly KpiContext _db;

    public DeleteIndicatorCommandHandler(KpiContext db)
    {
        _db = db;
    }

    protected override async Task Handle(DeleteIndicatorCommand request, CancellationToken cancellationToken)
    {
        var indicator = await _db.Indicators
            .FindAsync(new object?[] { request.IndicatorId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        _db.Indicators.Remove(indicator);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
