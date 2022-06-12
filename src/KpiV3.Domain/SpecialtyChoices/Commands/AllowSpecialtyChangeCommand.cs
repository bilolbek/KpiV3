using MediatR;

namespace KpiV3.Domain.SpecialtyChoices.Commands;

public record AllowSpecialtyChangeCommand : IRequest
{
    public Guid EmployeeId { get; init; }
    public Guid PeriodId { get; init; }
}

public class AllowSpecialtyChangeCommandHandler : AsyncRequestHandler<AllowSpecialtyChangeCommand>
{
    private readonly KpiContext _db;

    public AllowSpecialtyChangeCommandHandler(KpiContext db)
    {
        _db = db;
    }

    protected override async Task Handle(AllowSpecialtyChangeCommand request, CancellationToken cancellationToken)
    {
        var choice = await _db.SpecialtyChoices.FirstOrDefaultAsync(c =>
            c.EmployeeId == request.EmployeeId &&
            c.PeriodId == request.PeriodId, cancellationToken);

        if (choice is null)
        {
            return;
        }

        choice.CanBeChanged = true;

        await _db.SaveChangesAsync(cancellationToken);
    }
}
