using KpiV3.Domain.Common.DataContracts;
using KpiV3.Domain.Positions.DataContracts;
using MediatR;

namespace KpiV3.Domain.Positions.Queries;

public record GetPositionsQuery : IRequest<Page<Position>>
{
    public string? Name { get; init; }
    public Pagination Pagination { get; init; }
}

public class GetPositionsHandler : IRequestHandler<GetPositionsQuery, Page<Position>>
{
    private readonly KpiContext _db;

    public GetPositionsHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<Page<Position>> Handle(GetPositionsQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Positions
            .Include(p => p.Specialties)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            query = query.Where(p => p.Name.Contains(request.Name));
        }

        return await query.ToPageAsync(request.Pagination, cancellationToken);
    }
}
