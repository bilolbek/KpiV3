using KpiV3.Domain.Periods.DataContracts;
using MediatR;

namespace KpiV3.Domain.Periods.Queries;

public record GetPeriodQuery : IRequest<Period>
{
    public Guid PeriodId { get; init; }
}

public class GetPeriodQueryHandler : IRequestHandler<GetPeriodQuery, Period>
{
    private readonly KpiContext _db;

    public GetPeriodQueryHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<Period> Handle(GetPeriodQuery request, CancellationToken cancellationToken)
    {
        return await _db.Periods
            .Include(p => p.PeriodParts)
            .FirstOrDefaultAsync(p => p.Id == request.PeriodId, cancellationToken: cancellationToken)
            .EnsureFoundAsync();
    }
}
