using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Positions.DataContracts;
using MediatR;

namespace KpiV3.Domain.Positions.Queries;

public record GetPositionsQuery : IRequest<Result<Page<Position>, IError>>
{
    public string Name { get; init; } = default!;
    public Pagination Pagination { get; init; }
}
