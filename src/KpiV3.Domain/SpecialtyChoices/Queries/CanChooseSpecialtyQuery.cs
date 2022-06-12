using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KpiV3.Domain.SpecialtyChoices.Queries;

public record CanChooseSpecialtyQuery : IRequest<bool>
{
    public Guid EmployeeId { get; set; }
    public Guid PeriodId { get; set; }
}

public class CanChooseSpecialtyQueryHander : IRequestHandler<CanChooseSpecialtyQuery, bool>
{
    private readonly KpiContext _db;

    public CanChooseSpecialtyQueryHander(KpiContext db)
    {
        _db = db;
    }

    public async Task<bool> Handle(CanChooseSpecialtyQuery request, CancellationToken cancellationToken)
    {
        var choice = await _db.SpecialtyChoices
            .FirstOrDefaultAsync(sc =>
                sc.EmployeeId == request.EmployeeId &&
                sc.PeriodId == request.PeriodId, cancellationToken);

        return choice is null || choice.CanBeChanged;
    }
}
