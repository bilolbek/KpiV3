using KpiV3.Domain.Common.DataContracts;
using KpiV3.Domain.Indicators.DataContracts;
using MediatR;

namespace KpiV3.Domain.Indicators.Queries;

public record GetIndicatorsQuery : IRequest<Page<Indicator>>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Comment { get; set; }
    public Pagination Pagination { get; init; }
}

public class GetIndicatorsQueryHandler : IRequestHandler<GetIndicatorsQuery, Page<Indicator>>
{
    private readonly KpiContext _db;

    public GetIndicatorsQueryHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<Page<Indicator>> Handle(GetIndicatorsQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Indicators.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            query = query.Where(i => i.Name.Contains(request.Name));
        }

        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            query = query.Where(i => i.Description.Contains(request.Description));
        }

        if (!string.IsNullOrWhiteSpace(request.Comment))
        {
            query = query.Where(i => i.Comment!.Contains(request.Comment));
        }

        return await query.ToPageAsync(request.Pagination, cancellationToken);
    }
}
