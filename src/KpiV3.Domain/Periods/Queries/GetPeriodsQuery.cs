using KpiV3.Domain.Common.DataContracts;
using KpiV3.Domain.Periods.DataContracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KpiV3.Domain.Periods.Queries;

public record GetPeriodsQuery : IRequest<Page<Period>>
{
    public string? Name { get; init; }
    public Pagination Pagination { get; init; }
}

public class GetPeriodsQueryHandler : IRequestHandler<GetPeriodsQuery, Page<Period>>
{
    private readonly KpiContext _db;

    public GetPeriodsQueryHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<Page<Period>> Handle(GetPeriodsQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Periods
            .Include(p => p.PeriodParts)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            query = query.Where(p => p.Name.Contains(request.Name));
        }

        return await query.ToPageAsync(request.Pagination, cancellationToken);
    }
}
