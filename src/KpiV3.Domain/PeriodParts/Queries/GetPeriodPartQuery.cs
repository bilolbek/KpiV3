using KpiV3.Domain.PeriodParts.DataContracts;
using MediatR;

namespace KpiV3.Domain.PeriodParts.Queries;

public record GetPeriodPartQuery : IRequest<Result<PeriodPart, IError>>
{
    public Guid PartId { get; set; }
}
