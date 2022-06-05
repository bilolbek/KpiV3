using KpiV3.Domain.Requirements.DataContracts;
using MediatR;

namespace KpiV3.Domain.Requirements.Queries;

public record GetRequirementsQuery : IRequest<Result<List<Requirement>, IError>>
{
    public Guid PeriodId { get; set; }
    public Guid SpecialtyId { get; set; }
}
