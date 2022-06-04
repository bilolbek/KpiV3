using KpiV3.Domain.Positions.DataContracts;
using MediatR;

namespace KpiV3.Domain.Positions.Queries;

public record GetPositionQuery : IRequest<Result<Position, IError>>
{
    public Guid PositionId { get; set; }
}
