using KpiV3.Domain.PeriodParts.DataContracts;
using MediatR;

namespace KpiV3.Domain.PeriodParts.Queries;

public record GetPeriodPartsQuery : IRequest<List<PeriodPart>>
{
    public Guid PeriodId { get; init; }
}

public class GetPeriodPartsQueryHandler : IRequestHandler<GetPeriodPartsQuery, List<PeriodPart>>
{
    private readonly KpiContext _db;

    public GetPeriodPartsQueryHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<List<PeriodPart>> Handle(GetPeriodPartsQuery request, CancellationToken cancellationToken)
    {
        return await _db.PeriodParts
            .AsNoTracking()
            .Where(p => p.PeriodId == request.PeriodId)
            .ToListAsync(cancellationToken);
    }
}
