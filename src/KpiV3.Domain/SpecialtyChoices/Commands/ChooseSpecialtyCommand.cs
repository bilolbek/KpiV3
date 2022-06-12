using KpiV3.Domain.SpecialtyChoices.DataContracts;
using MediatR;

namespace KpiV3.Domain.SpecialtyChoices.Commands;

public record ChooseSpecialtyCommand : IRequest
{
    public Guid EmployeeId { get; init; }
    public Guid SpecialtyId { get; init; }
    public Guid PeriodId { get; init; }
}

public class ChooseSpecialtyCommandHandler : AsyncRequestHandler<ChooseSpecialtyCommand>
{
    private readonly KpiContext _db;

    public ChooseSpecialtyCommandHandler(KpiContext db)
    {
        _db = db;
    }

    protected override async Task Handle(ChooseSpecialtyCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);

        var choice = await _db.SpecialtyChoices
            .FirstOrDefaultAsync(c =>
                c.EmployeeId == request.EmployeeId &&
                c.PeriodId == request.PeriodId, cancellationToken);

        if (choice is null)
        {
            _db.SpecialtyChoices.Add(new SpecialtyChoice
            {
                EmployeeId = request.EmployeeId,
                SpecialtyId = request.SpecialtyId,
                PeriodId = request.PeriodId,
                CanBeChanged = false,
            });
        }
        else
        {
            if (!choice.CanBeChanged)
            {
                throw new BusinessLogicException(
                    "You cannot change your specialty for this period. If you want to change your specialty, please contact your administrator.");
            }

            choice.SpecialtyId = request.SpecialtyId;
            choice.CanBeChanged = false;
        }

        await _db.SaveChangesAsync(cancellationToken);
        
        await transaction.CommitAsync(cancellationToken);
    }
}
