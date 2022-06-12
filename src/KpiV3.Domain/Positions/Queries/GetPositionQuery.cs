using KpiV3.Domain.Positions.DataContracts;
using MediatR;

namespace KpiV3.Domain.Positions.Queries;

public record GetPositionQuery : IRequest<Position>
{
    public Guid PositionId { get; set; }
}

public class GetPositionQueryHandler : IRequestHandler<GetPositionQuery, Position>
{
    private readonly KpiContext _db;

    public GetPositionQueryHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<Position> Handle(GetPositionQuery request, CancellationToken cancellationToken)
    {
        return await _db.Positions
            .FindAsync(new object?[] { request.PositionId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();
    }
}
