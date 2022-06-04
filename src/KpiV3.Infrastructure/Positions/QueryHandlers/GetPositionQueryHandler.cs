using KpiV3.Domain.Positions.DataContracts;
using KpiV3.Domain.Positions.Queries;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Positions.Data;
using MediatR;

namespace KpiV3.Infrastructure.Positions.QueryHandlers;

internal class GetPositionQueryHandler : IRequestHandler<GetPositionQuery, Result<Position, IError>>
{
    private readonly Database _db;

    public GetPositionQueryHandler(Database db)
    {
        _db = db;
    }

    public async Task<Result<Position, IError>> Handle(GetPositionQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"SELECT * FROM positions WHERE id = @PositionId";

        return await _db
            .QueryFirstAsync<PositionRow>(new(sql, new { request.PositionId }))
            .MapAsync(row => row.ToModel());
    }
}
