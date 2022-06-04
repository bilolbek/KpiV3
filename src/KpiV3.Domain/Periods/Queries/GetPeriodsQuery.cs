using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Periods.DataContracts;
using MediatR;

namespace KpiV3.Domain.Periods.Queries;

public record GetPeriodsQuery : IRequest<Result<Page<Period>, IError>>
{
    public Pagination Pagination { get; set; }
}