using KpiV3.Domain.Common.DataContracts;
using KpiV3.Domain.PeriodParts.DataContracts;
using MediatR;

namespace KpiV3.Domain.PeriodParts.Commands;

public record UpdatePeriodPartCommand : IRequest<PeriodPart>
{
    public Guid PartId { get; init; }
    public string Name { get; init; } = default!;
    public DateRange Range { get; init; } = default!;
}

public class UpdatePeriodPartCommandHandler : IRequestHandler<UpdatePeriodPartCommand, PeriodPart>
{
    private readonly KpiContext _db;

    public UpdatePeriodPartCommandHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<PeriodPart> Handle(UpdatePeriodPartCommand request, CancellationToken cancellationToken)
    {
        var part = await _db.PeriodParts
            .Include(p => p.Period)
            .FirstOrDefaultAsync(p => p.Id == request.PartId, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        EnsurePartIsInsidePeriodAsync(part, request);

        part.Name = request.Name;
        part.Range = request.Range;

        await _db.SaveChangesAsync(cancellationToken);

        return part;
    }

    private void EnsurePartIsInsidePeriodAsync(PeriodPart periodPart, UpdatePeriodPartCommand request)
    {
        if (!periodPart.Period.Includes(request.Range))
        {
            throw new InvalidInputException("Give range does not lie inside given period range");
        }
    }
}
