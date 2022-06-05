using KpiV3.Domain.Periods.DataContracts;
using MediatR;

namespace KpiV3.Domain.Periods.Queries;

public record GetActivePeriodQuery : IRequest<Result<Period, IError>>
{
    public DateTimeOffset Now { get; set; }
}
