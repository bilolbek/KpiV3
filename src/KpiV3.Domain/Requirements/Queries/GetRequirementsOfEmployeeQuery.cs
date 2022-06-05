using KpiV3.Domain.Requirements.DataContracts;
using MediatR;

namespace KpiV3.Domain.Requirements.Queries;

public record GetRequirementsOfEmployeeQuery : IRequest<Result<List<Requirement>, IError>>
{
    public Guid EmployeeId { get; set; }
    public Guid PeriodId { get; set; }
}
