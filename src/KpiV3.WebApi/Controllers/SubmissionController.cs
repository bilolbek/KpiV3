using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Submissions.Commands;
using KpiV3.Domain.Submissions.DataContracts;
using KpiV3.Domain.Submissions.Queries;
using KpiV3.WebApi.Authentication;
using KpiV3.WebApi.DataContracts.Submissions;
using KpiV3.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("3.0")]
public class SubmissionController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IEmployeeAccessor _employeeAccessor;

    public SubmissionController(
        IMediator mediator,
        IEmployeeAccessor employeeAccessor)
    {
        _mediator = mediator;
        _employeeAccessor = employeeAccessor;
    }

    [HttpGet]
    [Authorize(Policy = "RootOnly")]
    [ProducesResponseType(200, Type = typeof(Page<SubmissionDto>))]
    public async Task<IActionResult> GetAsync([FromQuery] GetSubmissionsByStatusRequest request)
    {
        return await _mediator
            .Send(request.ToQuery())
            .MapAsync(s => s.Map(s => new SubmissionDto(s)))
            .MatchAsync(s => Ok(s), error => error.MapToActionResult());
    }

    [Authorize(Policy = "RootOnly")]
    [HttpGet("{employeeId:guid}/{requirementId:guid}")]
    [ProducesResponseType(200, Type = typeof(SubmissionDto))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetEmployeeSubmissionsAsync(Guid employeeId, Guid requirementId)
    {
        return await _mediator
            .Send(new GetSubmissionsQuery { EmployeeId = employeeId, RequirementId = requirementId })
            .MapAsync(submissions => submissions.Select(s => new SubmissionDto(s)).ToList())
            .MatchAsync(submissions => Ok(submissions), error => error.MapToActionResult());
    }

    [Authorize(Policy = "RootOnly")]
    [HttpPut("{submissionId:guid}/{status}")]
    public async Task<IActionResult> UpdateStatusAsync(Guid submissionId, SubmissionStatus status)
    {
        return await _mediator
            .Send(new UpdateSubmissionStatusCommand { SubmissionId = submissionId, Status = status })
            .MatchAsync(() => Ok(), error => error.MapToActionResult());
    }

    [HttpGet("{requirementId:guid}")]
    [ProducesResponseType(200, Type = typeof(SubmissionDto))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByRequirementIdAsync(Guid requirementId)
    {
        return await _mediator
            .Send(new GetSubmissionsQuery { EmployeeId = _employeeAccessor.EmployeeId, RequirementId = requirementId })
            .MapAsync(submissions => submissions.Select(s => new SubmissionDto(s)).ToList())
            .MatchAsync(submissions => Ok(submissions), error => error.MapToActionResult());
    }

    [HttpPost]
    [ProducesResponseType(200, Type = typeof(SubmissionDto))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateSubmissionRequest request)
    {
        return await _mediator
            .Send(request.ToCommand(_employeeAccessor.EmployeeId))
            .MapAsync(submission => new SubmissionDto(submission))
            .MatchAsync(submission => Ok(submission), error => error.MapToActionResult());
    }

    [HttpPut("{submissionId:guid}")]
    [ProducesResponseType(200, Type = typeof(SubmissionDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateAsync(Guid submissionId, [FromBody] UpdateSubmissionRequest request)
    {
        return await _mediator
            .Send(request.ToCommand(_employeeAccessor.EmployeeId, submissionId))
            .MapAsync(submission => new SubmissionDto(submission))
            .MatchAsync(submission => Ok(submission), error => error.MapToActionResult());
    }

    [HttpDelete("{submissionId:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteAsync(Guid submissionId)
    {
        return await _mediator
            .Send(new DeleteSubmissionCommand { IdOfWhoWantsToDelete = _employeeAccessor.EmployeeId, SubmissionId = submissionId })
            .MatchAsync(() => Ok(), error => error.MapToActionResult());
    }
}
