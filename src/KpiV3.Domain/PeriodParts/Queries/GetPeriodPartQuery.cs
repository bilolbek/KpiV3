using KpiV3.Domain.PeriodParts.DataContracts;
using MediatR;

namespace KpiV3.Domain.PeriodParts.Queries;

public record GetPeriodPartQuery : IRequest<PeriodPart>
{
    public Guid PeriodPartId { get; init; }
}

public class GetPeriodPartQueryHandler : IRequestHandler<GetPeriodPartQuery, PeriodPart>
{
    private readonly KpiContext _db;

    public GetPeriodPartQueryHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<PeriodPart> Handle(GetPeriodPartQuery request, CancellationToken cancellationToken)
    {
        return await _db.PeriodParts
            .FindAsync(new object?[] { request.PeriodPartId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();
    }
}
