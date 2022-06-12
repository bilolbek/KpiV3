using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Submissions.Commands;
using KpiV3.Domain.Submissions.DataContracts;
using KpiV3.Domain.Submissions.Queries;
using KpiV3.WebApi.Authentication.Services;
using KpiV3.WebApi.DataContracts.Submissions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

namespace KpiV3.WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
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


    [HttpGet("{requirementId:guid}")]
    public async Task<IActionResult> GetAsync(Guid requirementId)
    {
        var submission = await _mediator.Send(new GetSubmissionQuery
        {
            EmployeeId = _employeeAccessor.EmployeeId,
            RequirementId = requirementId,
        });

        return Ok(new SubmissionDto(submission));
    }

    [Authorize(Policy = "RootOnly")]
    [HttpGet("{employeeId:guid}/{requirementId:guid}")]
    public async Task<IActionResult> GetAsync(Guid employeeId, Guid requirementId)
    {
        var submission = await _mediator.Send(new GetSubmissionQuery
        {
            EmployeeId = employeeId,
            RequirementId = requirementId,
        });

        return Ok(new SubmissionDto(submission));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateSubmissionRequest request)
    {
        var submission = await _mediator.Send(request.ToCommand(_employeeAccessor.EmployeeId));

        return Ok(new SubmissionDto(submission));
    }

    [HttpPut("{submissionId:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid submissionId, [FromBody] UpdateSubmissionsRequest request)
    {
        var submission = await _mediator.Send(request.ToCommand(submissionId, _employeeAccessor.EmployeeId));

        return Ok(new SubmissionDto(submission));
    }

    [HttpDelete("{submissionId:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid submissionId)
    {
        await _mediator.Send(new DeleteSubmissionCommand
        {
            IdOfWhoWantsToDelete = _employeeAccessor.EmployeeId,
            SubmissionId = submissionId,
        });

        return Ok();
    }
}
