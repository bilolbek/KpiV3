using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Queries;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Employees.Data;
using MediatR;

namespace KpiV3.Infrastructure.Employees.QueryHandlers;

internal class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, Result<Profile, IError>>
{
    private readonly Database _db;

    public GetProfileQueryHandler(Database db)
    {
        _db = db;
    }

    public async Task<Result<Profile, IError>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
SELECT
    e.id,
    e.first_name,
    e.last_name,
    e.middle_name,
    e.email,
    e.avatar_id,
    p.id as ""position_id"",
    p.name as ""position_name"",
    p.type as ""position_type""
FROM employees e
INNER JOIN positions p on e.position_id = p.id
WHERE e.id = @EmployeeId";

        return await _db
            .QueryFirstAsync<ProfileRow>(new(sql, new { request.EmployeeId }))
            .MapAsync(row => row.ToModel());
    }
}
