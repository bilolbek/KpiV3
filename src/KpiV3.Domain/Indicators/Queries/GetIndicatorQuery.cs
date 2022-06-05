using KpiV3.Domain.Indicators.DataContracts;
using MediatR;

namespace KpiV3.Domain.Indicators.Queries;

public record GetIndicatorQuery : IRequest<Result<Indicator, IError>>
{
    public Guid IndicatorId { get; set; }
}