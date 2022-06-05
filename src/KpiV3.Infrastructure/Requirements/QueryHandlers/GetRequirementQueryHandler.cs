using KpiV3.Domain.Requirements.DataContracts;
using KpiV3.Domain.Requirements.Queries;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Requirements.Data;
using MediatR;

namespace KpiV3.Infrastructure.Requirements.QueryHandlers;

internal class GetRequirementQueryHandler : IRequestHandler<GetRequirementQuery, Result<Requirement, IError>>
{
    private readonly Database _db;

    public GetRequirementQueryHandler(Database db)
    {
        _db = db;
    }

    public async Task<Result<Requirement, IError>> Handle(GetRequirementQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
SELECT * FROM requirements
WHERE id = @RequirementId";

        return await _db
            .QueryFirstAsync<RequirementRow>(new(sql, new { request.RequirementId }))
            .MapAsync(row => row.ToModel());
    }
}
