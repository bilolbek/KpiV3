using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Positions.DataContracts;
using KpiV3.Domain.Positions.Queries;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Positions.Data;
using MediatR;

namespace KpiV3.Infrastructure.Positions.QueryHandlers;

internal class GetPositionsQueryHandler : IRequestHandler<GetPositionsQuery, Result<Page<Position>, IError>>
{
    private readonly Database _db;

    public GetPositionsQueryHandler(Database db)
    {
        _db = db;
    }

    public async Task<Result<Page<Position>, IError>> Handle(GetPositionsQuery request, CancellationToken cancellationToken)
    {
        const string countSql = @"
SELECT
    COUNT(*)
FROM positions
WHERE name LIKE @Name";

        const string selectSql = @"
SELECT
    *
FROM positions
WHERE name LIKE @Name
ORDER BY name
LIMIT @Limit OFFSET @Offset";

        return await _db
            .QueryFirstAsync<int>(new(countSql, new { Name = $"%{request.Name}%" }))
            .BindAsync(total => _db
                .QueryAsync<PositionRow>(new(selectSql, new
                {
                    Name = $"%{request.Name}%",
                    request.Pagination.Limit,
                    request.Pagination.Offset,
                })).MapAsync(rows => new Page<PositionRow>(total, request.Pagination, rows)))
            .MapAsync(page => page.Map(row => row.ToModel()));
    }
}
