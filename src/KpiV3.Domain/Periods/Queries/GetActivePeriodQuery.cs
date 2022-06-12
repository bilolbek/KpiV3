using KpiV3.Domain.Periods.DataContracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KpiV3.Domain.Periods.Queries;

public record GetActivePeriodQuery : IRequest<Period>
{
}

public class GetActivePeriodQueryHandler : IRequestHandler<GetActivePeriodQuery, Period>
{
    private readonly KpiContext _db;
    private readonly IDateProvider _dateProvider;

    public GetActivePeriodQueryHandler(
        KpiContext db, 
        IDateProvider dateProvider)
    {
        _db = db;
        _dateProvider = dateProvider;
    }

    public async Task<Period> Handle(GetActivePeriodQuery request, CancellationToken cancellationToken)
    {
        var now = _dateProvider.Now();

        return await _db.Periods
            .FirstOrDefaultAsync(p => now >= p.Range.From && now <= p.Range.To);
    }
}
