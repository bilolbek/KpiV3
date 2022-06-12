using KpiV3.Domain.Specialties.DataContracts;
using MediatR;

namespace KpiV3.Domain.SpecialtyChoices.Queries;

public record GetChosenSpecialtyQuery : IRequest<Specialty>
{
    public Guid EmployeeId { get; init; }
    public Guid PeriodId { get; init; }
}

public class GetChosenSpecialtyQueryHandler : IRequestHandler<GetChosenSpecialtyQuery, Specialty>
{
    private readonly KpiContext _db;

    public GetChosenSpecialtyQueryHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<Specialty> Handle(GetChosenSpecialtyQuery request, CancellationToken cancellationToken)
    {
        return await _db.SpecialtyChoices
            .Where(c =>
                c.EmployeeId == request.EmployeeId &&
                c.PeriodId == request.PeriodId)
            .Select(c => c.Specialty)
            .FirstOrDefaultAsync(cancellationToken)
            .EnsureFoundAsync();
    }
}
