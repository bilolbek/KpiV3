using KpiV3.Domain.Employees.Queries;
using KpiV3.Domain.SpecialtyChoices.Queries;
using KpiV3.Domain.Tasklists.Queries;
using KpiV3.WebApi.Authentication.Services;
using KpiV3.WebApi.DataContracts.Common;
using KpiV3.WebApi.DataContracts.Profiles;
using KpiV3.WebApi.DataContracts.Specialties;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IEmployeeAccessor _employeeAccessor;

    public ProfileController(
        IMediator mediator,
        IEmployeeAccessor employeeAccessor)
    {
        _mediator = mediator;
        _employeeAccessor = employeeAccessor;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ProfileDto))]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        var profile = await _mediator.Send(new GetProfileQuery
        {
            EmployeeId = _employeeAccessor.EmployeeId
        }, cancellationToken);

        return Ok(new ProfileDto(profile));
    }

    [HttpPut]
    [ProducesResponseType(200)]
    public async Task<IActionResult> UpdateProfileAsync([FromBody] UpdateProfileRequest request)
    {
        await _mediator.Send(request.ToCommand(_employeeAccessor.EmployeeId));

        return Ok();
    }

    [HttpGet("{employeeId:guid}")]
    [ProducesResponseType(200, Type = typeof(ProfileDto))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetAsync(Guid employeeId)
    {
        var profile = await _mediator.Send(new GetProfileQuery { EmployeeId = employeeId });

        return Ok(new ProfileDto(profile));
    }

    [HttpPost("change-password")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
    {
        await _mediator.Send(request.ToCommand(_employeeAccessor.EmployeeId));

        return Ok();
    }

    [HttpGet("specialty/{periodId:guid}")]
    [ProducesResponseType(200, Type = typeof(SpecialtyDto))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetChosenSpecialtyAsync(Guid periodId)
    {
        var specialty = await _mediator
            .Send(new GetChosenSpecialtyQuery { PeriodId = periodId, EmployeeId = _employeeAccessor.EmployeeId });

        return Ok(new SpecialtyDto(specialty));
    }

    [HttpGet("can-choose-specialty/{periodId:guid}")]
    public async Task<IActionResult> CanChooseSpecialtyAsync(Guid periodId)
    {
        return Ok(await _mediator.Send(new CanChooseSpecialtyQuery
        {
            PeriodId = periodId,
            EmployeeId = _employeeAccessor.EmployeeId
        }));
    }

    [HttpPost("specialty")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ChooseSpecialtyAsync([FromBody] ChooseSpecialtyRequest request)
    {
        await _mediator.Send(request.ToCommand(_employeeAccessor.EmployeeId));

        return Ok();
    }

    [HttpGet("tasks/{periodId}")]
    public async Task<IActionResult> GetTasksAsync(Guid periodId)
    {
        return Ok(await _mediator.Send(new GetEmployeeTasksQuery
        {
            EmployeeId = _employeeAccessor.EmployeeId,
            PeriodId = periodId
        }));
    }
}