using KpiV3.Domain.Indicators.DataContracts;
using MediatR;

namespace KpiV3.Domain.Indicators.Queries;

public record GetIndicatorQuery : IRequest<Indicator>
{
    public Guid IndicatorId { get; init; }
}

public class GetIndicatorQueryHandler : IRequestHandler<GetIndicatorQuery, Indicator>
{
    private readonly KpiContext _db;

    public GetIndicatorQueryHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<Indicator> Handle(GetIndicatorQuery request, CancellationToken cancellationToken)
    {
        return await _db.Indicators
            .FindAsync(new object?[] { request.IndicatorId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();
    }
}
