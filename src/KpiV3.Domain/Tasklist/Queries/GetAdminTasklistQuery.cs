using KpiV3.Domain.Common.DataContracts;
using KpiV3.Domain.Tasklist.DataContracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KpiV3.Domain.Tasklist.Queries;

public record GetAdminTasklistQuery : IRequest<Page<AdminTasklistRow>>
{
    public Guid PeriodId { get; init; }
    public Pagination Pagination { get; init; }
}

public class GetAdminTasklistQueryHandler : IRequestHandler<GetAdminTasklistQuery, Page<AdminTasklistRow>>
{
    private readonly KpiContext _db;

    public GetAdminTasklistQueryHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<Page<AdminTasklistRow>> Handle(GetAdminTasklistQuery request, CancellationToken cancellationToken)
    {
        return await _db.Employees
            .Select(e => new AdminTasklistRow
            {
                Employee = new Profile
                {
                    Id = e.Id,
                    Email = e.Email,
                    Name = new()
                    {
                        FirstName = e.Name.FirstName,
                        LastName = e.Name.LastName,
                        MiddleName = e.Name.MiddleName,
                    },
                    AvatarId = e.AvatarId,
                },

                SpecialtyId = e.SpecialtyChoices
                    .Where(sc => sc.PeriodId == request.PeriodId)
                    .Select(sc => (Guid?)sc.SpecialtyId)
                    .FirstOrDefault(),

                SubmissionsCount = _db.Submissions
                    .Count(s =>
                        s.EmployeeId == e.Id &&
                        s.Requirement.PeriodPart.PeriodId == request.PeriodId),

                TotalGrade = _db.Grades
                    .Where(g => g.EmployeeId == e.Id &&
                                g.Requirement.PeriodPart.PeriodId == request.PeriodId)
                    .Sum(g => g.Value),
            })
            .Select(e => new AdminTasklistRow
            {
                Employee = e.Employee,
                SpecialtyId = e.SpecialtyId,
                SubmissionsCount = e.SubmissionsCount,
                TotalGrade = e.TotalGrade,
                TotalWeight = e.SpecialtyId.HasValue ? _db.Requirements
                    .Where(r => r.SpecialtyId == e.SpecialtyId &&
                                r.PeriodPart.PeriodId == request.PeriodId)
                    .Sum(r => r.Weight) : 0,
            })
            .ToPageAsync(request.Pagination, cancellationToken);
    }
}
