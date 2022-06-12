using MediatR;

namespace KpiV3.Domain.Periods.Commands;

public record DeletePeriodCommand : IRequest
{
    public Guid PeriodId { get; init; }
}

public class DeletePeriodCommandHandler : AsyncRequestHandler<DeletePeriodCommand>
{
    private readonly KpiContext _db;

    public DeletePeriodCommandHandler(KpiContext db)
    {
        _db = db;
    }

    protected override async Task Handle(DeletePeriodCommand request, CancellationToken cancellationToken)
    {
        var period = await _db.Periods
            .FindAsync(new object?[] { request.PeriodId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        _db.Periods.Remove(period);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
