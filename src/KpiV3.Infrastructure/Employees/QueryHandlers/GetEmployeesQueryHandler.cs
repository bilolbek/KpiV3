using Dapper;
using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Queries;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Employees.Data;
using MediatR;

namespace KpiV3.Infrastructure.Employees.QueryHandlers;

internal class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, Result<Page<Profile>, IError>>
{
    private readonly Database _db;

    public GetEmployeesQueryHandler(Database db)
    {
        _db = db;
    }

    public async Task<Result<Page<Profile>, IError>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        return await _db
            .QueryFirstAsync<int>(Count(request))
            .BindAsync(total =>
                _db.QueryAsync<ProfileRow>(Select(request))
                    .MapAsync(rows => new Page<ProfileRow>(total, request.Pagination, rows)))
            .MapAsync(rows => rows.Map(r => r.ToModel()));
    }

    private CommandDefinition Count(GetEmployeesQuery request)
    {
        return request.PositionId.HasValue ?
            CountWithFilter(request) :
            CountWithoutFilter();
    }

    private CommandDefinition CountWithoutFilter()
    {
        return new(@"SELECT COUNT(*) FROM employees");
    }

    private CommandDefinition CountWithFilter(GetEmployeesQuery request)
    {
        return new(@"SELECT COUNT(*) FROM employees WHERE position_id = @PositionId", new { request.PositionId });
    }

    private CommandDefinition Select(GetEmployeesQuery request)
    {
        return request.PositionId.HasValue ?
            SelectWithFilter(request) :
            SelectWithoutFilter(request);
    }

    private CommandDefinition SelectWithoutFilter(GetEmployeesQuery request)
    {
        return new(@"
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
ORDER BY e.last_name
LIMIT @Limit OFFSET @Offset", new { request.Pagination.Limit, request.Pagination.Offset });
    }

    private CommandDefinition SelectWithFilter(GetEmployeesQuery request)
    {
        return new(@"
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
WHERE e.position_id = @PositionId
ORDER BY e.last_name
LIMIT @Limit OFFSET @Offset", new { request.Pagination.Limit, request.Pagination.Offset, request.PositionId });
    }
}
