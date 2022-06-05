using KpiV3.Domain.Grades.DataContracts;
using KpiV3.Domain.Grades.Queries;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Grades.Data;
using MediatR;

namespace KpiV3.Infrastructure.Grades.QueryHandlers;

internal class GetGradeQueryHandler : IRequestHandler<GetGradeQuery, Result<Grade, IError>>
{
    private readonly Database _db;

    public GetGradeQueryHandler(Database db)
    {
        _db = db;
    }

    public async Task<Result<Grade, IError>> Handle(GetGradeQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
SELECT * FROM grades
WHERE employee_id = @EmployeeId AND requirement_id = @RequirementId";

        return await _db
            .QueryFirstAsync<GradeRow>(new(sql, new { request.EmployeeId, request.RequirementId }))
            .MapAsync(row => row.ToModel());
    }
}
