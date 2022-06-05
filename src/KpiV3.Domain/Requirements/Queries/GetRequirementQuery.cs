using KpiV3.Domain.Requirements.DataContracts;
using MediatR;

namespace KpiV3.Domain.Requirements.Queries;

public record GetRequirementQuery : IRequest<Result<Requirement, IError>>
{
    public Guid RequirementId { get; set; }
}
