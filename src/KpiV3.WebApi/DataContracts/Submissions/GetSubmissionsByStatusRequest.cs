using KpiV3.Domain.Submissions.DataContracts;
using KpiV3.Domain.Submissions.Queries;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KpiV3.WebApi.DataContracts.Submissions;

public class GetSubmissionsByStatusRequest
{
    [FromQuery]
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; }

    [FromQuery]
    [Range(1, int.MaxValue)]
    public int PageSize { get; set; }

    public SubmissionStatus? Status { get; set; }

    public GetSubmissionsByStatusQuery ToQuery()
    {
        return new GetSubmissionsByStatusQuery
        {
            Status = Status,
            Pagination = new()
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
            },
        };
    }
}
