using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Indicators.DataContracts;
using MediatR;

namespace KpiV3.Domain.Indicators.Queries;

public record GetIndicatorsQuery : IRequest<Result<Page<Indicator>, IError>>
{
    public Pagination Pagination { get; set; }
}
