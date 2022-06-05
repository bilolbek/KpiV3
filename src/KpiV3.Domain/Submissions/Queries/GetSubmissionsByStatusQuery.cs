using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Submissions.DataContracts;
using MediatR;

namespace KpiV3.Domain.Submissions.Queries;

public record GetSubmissionsByStatusQuery : IRequest<Result<Page<Submission>, IError>>
{
    public Pagination Pagination { get; set; }
    public SubmissionStatus? Status { get; set; }
}
