using KpiV3.Domain.PeriodParts.DataContracts;
using MediatR;

namespace KpiV3.Domain.PeriodParts.Queries;

public class GetPeriodPartsQuery : IRequest<Result<List<PeriodPart>, IError>>
{
    public Guid PeriodId { get; set; }
}
