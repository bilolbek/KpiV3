using KpiV3.Domain.Specialties.DataContracts;
using KpiV3.Domain.Specialties.Queries;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Specialties.Data;
using MediatR;

namespace KpiV3.Infrastructure.Specialties.QueryHandlers;

internal class GetChoosenSpecialtyQueryHandler : IRequestHandler<GetChoosenSpecialtyQuery, Result<Specialty, IError>>
{
    private readonly Database _db;

    public GetChoosenSpecialtyQueryHandler(Database db)
    {
        _db = db;
    }

    public async Task<Result<Specialty, IError>> Handle(GetChoosenSpecialtyQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
SELECT s.* FROM employees e
INNER JOIN positions p on e.position_id = p.id
INNER JOIN specialties s on s.position_id = p.id
INNER JOIN specialty_choices sp on sp.specialty_id = s.id

WHERE sp.period_id = @PeriodId AND e.id = @EmployeeId";

        return await _db
            .QueryFirstAsync<SpecialtyRow>(new(sql, new { request.PeriodId, request.EmployeeId }))
            .MapAsync(row => row.ToModel());
    }
}
