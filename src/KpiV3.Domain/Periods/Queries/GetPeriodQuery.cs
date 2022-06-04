using KpiV3.Domain.Periods.DataContracts;
using MediatR;

namespace KpiV3.Domain.Periods.Queries;

public record GetPeriodQuery : IRequest<Result<Period, IError>>
{
    public Guid PeriodId { get; set; }
}
