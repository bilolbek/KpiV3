using KpiV3.Domain.Reports.DataContracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KpiV3.Domain.Reports.Queries;

public record GetReportsQuery : IRequest<Report>
{
    public Guid PeriodId { get; init; }
    public List<Guid> EmployeeIds { get; init; } = default!;
}

public class GetReportsQueryHandler : IRequestHandler<GetReportsQuery, Report>
{
    private readonly KpiContext _db;
    private readonly IDateProvider _dateProvider;

    public GetReportsQueryHandler(
        KpiContext db,
        IDateProvider dateProvider)
    {
        _db = db;
        _dateProvider = dateProvider;
    }

    public async Task<Report> Handle(GetReportsQuery request, CancellationToken cancellationToken)
    {
        var period = await _db.Periods
            .FindAsync(new object?[] { request.PeriodId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        var query = _db.SpecialtyChoices
            .Where(sc => sc.PeriodId == request.PeriodId);

        if (request.EmployeeIds.Any())
        {
            query = query.Where(sc => request.EmployeeIds.Contains(sc.EmployeeId));
        }

        var employeeReports = await query
            .Select(sc => new EmployeeReport
            {
                FullName =
                    sc.Employee.Name.FirstName +
                    " " +
                    sc.Employee.Name.LastName +
                    (sc.Employee.Name.MiddleName != null ? " " + sc.Employee.Name.MiddleName : ""),

                Position = sc.Employee.Position.Name,

                Specialty = sc.Specialty.Name,

                Items = sc.Specialty.Requirements.Select(r => new ReportItem
                {
                    Indicator = r.Indicator.Name,
                    Weight = r.Weight,
                    Value = r.Grades.Any(g => g.EmployeeId == sc.EmployeeId) ?
                        r.Grades.Where(g => g.EmployeeId == sc.EmployeeId).Sum(g => g.Value) :
                        null,
                }).ToList(),
            })
            .ToListAsync(cancellationToken);


        return new Report
        {
            EmployeeReports = employeeReports,

            Period = period.Name,

            CreatedDate = _dateProvider.Now(),
        };
    }
}
