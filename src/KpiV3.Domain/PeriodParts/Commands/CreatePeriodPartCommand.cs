using KpiV3.Domain.Common.DataContracts;
using KpiV3.Domain.PeriodParts.DataContracts;
using MediatR;

namespace KpiV3.Domain.PeriodParts.Commands;

public record CreatePeriodPartCommand : IRequest<PeriodPart>
{
    public Guid PeriodId { get; init; }
    public string Name { get; init; } = default!;
    public DateRange Range { get; init; } = default!;
}

public class CreatePeriodPartCommandHandler : IRequestHandler<CreatePeriodPartCommand, PeriodPart>
{
    private readonly KpiContext _db;
    private readonly IGuidProvider _guidProvider;

    public CreatePeriodPartCommandHandler(
        KpiContext db,
        IGuidProvider guidProvider)
    {
        _db = db;
        _guidProvider = guidProvider;
    }

    public async Task<PeriodPart> Handle(CreatePeriodPartCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);

        await EnsurePartIsInsidePeriodAsync(request, cancellationToken);

        var part = new PeriodPart
        {
            Id = _guidProvider.New(),
            Name = request.Name,
            Range = request.Range,
            PeriodId = request.PeriodId,
        };

        _db.PeriodParts.Add(part);

        await _db.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return part;
    }

    private async Task EnsurePartIsInsidePeriodAsync(CreatePeriodPartCommand request, CancellationToken cancellationToken)
    {
        var period = await _db.Periods
            .FindAsync(new object?[] { request.PeriodId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        if (!period.Includes(request.Range))
        {
            throw new InvalidInputException("Give range does not lie inside given period range");
        }
    }
}
