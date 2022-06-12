using KpiV3.Domain.Tasklists.DataContracts;
using MediatR;

namespace KpiV3.Domain.Tasklists.Queries;

public record GetEmployeeTasksQuery : IRequest<List<EmployeeTask>>
{
    public Guid EmployeeId { get; init; }
    public Guid PeriodId { get; init; }
}

public class GetEmployeeTasksQueryHandler : IRequestHandler<GetEmployeeTasksQuery, List<EmployeeTask>>
{
    private readonly KpiContext _db;

    public GetEmployeeTasksQueryHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<List<EmployeeTask>> Handle(GetEmployeeTasksQuery request, CancellationToken cancellationToken)
    {
        var specialtyOfEmployee = await _db.SpecialtyChoices
            .FirstOrDefaultAsync(sc =>
                sc.EmployeeId == request.EmployeeId &&
                sc.PeriodId == request.PeriodId, cancellationToken);

        if (specialtyOfEmployee is null)
        {
            return new List<EmployeeTask>();
        }

        return await _db.Requirements
            .Where(r => r.PeriodPart.PeriodId == request.PeriodId && r.SpecialtyId == specialtyOfEmployee.SpecialtyId)
            .Select(r => new EmployeeTask
            {
                RequirementId = r.Id,
                IndicatorId = r.IndicatorId,
                IndicatorName = r.Indicator.Name,
                PeriodPartId = r.PeriodPartId,
                PeriodPartName = r.PeriodPart.Name,
                Grade = _db.Grades
                    .Where(g => g.EmployeeId == request.EmployeeId && g.RequirementId == r.Id)
                    .Select(g => (double?)g.Value)
                    .FirstOrDefault(),
                Weight = r.Weight,
                HasSubmission = r.Submissions.Any(s => s.EmployeeId == request.EmployeeId),
            })
            .ToListAsync(cancellationToken);
    }
}
