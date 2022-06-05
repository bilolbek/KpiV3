using KpiV3.Domain.Submissions.DataContracts;
using MediatR;

namespace KpiV3.Domain.Submissions.Queries;

public record GetSubmissionsQuery : IRequest<Result<List<Submission>, IError>>
{
    public Guid RequirementId { get; set; }
    public Guid EmployeeId { get; set; }
}