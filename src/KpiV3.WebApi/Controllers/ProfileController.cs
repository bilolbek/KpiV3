using KpiV3.Domain.Employees.Queries;
using KpiV3.Domain.Specialties.Queries;
using KpiV3.WebApi.Authentication;
using KpiV3.WebApi.DataContracts.Employees;
using KpiV3.WebApi.DataContracts.Profiles;
using KpiV3.WebApi.DataContracts.Specialties;
using KpiV3.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[ApiVersion("3.0")]
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
    public async Task<IActionResult> GetAsync()
    {
        return await _mediator
            .Send(new GetProfileQuery { EmployeeId = _employeeAccessor.EmployeeId })
            .MapAsync(profile => new ProfileDto(profile))
            .MatchAsync(profile => Ok(profile), error => error.MapToActionResult());
    }

    [HttpPut]
    [ProducesResponseType(200)]
    public async Task<IActionResult> UpdateProfileAsync([FromBody] UpdateProfileRequest request)
    {
        return await _mediator
            .Send(request.ToCommand(_employeeAccessor.EmployeeId))
            .MatchAsync(() => Ok(), error => error.MapToActionResult());
    }

    [HttpPost("change-password")]
    [ProducesResponseType(200)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
    {
        return await _mediator
            .Send(request.ToCommand(_employeeAccessor.EmployeeId))
            .MatchAsync(() => Ok(), error => error.MapToActionResult());
    }

    [HttpGet("specialty/{periodId:guid}")]
    [ProducesResponseType(200, Type = typeof(SpecialtyDto))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetChoosenSpecialtyAsync(Guid periodId)
    {
        return await _mediator
            .Send(new GetChoosenSpecialtyQuery { PeriodId = periodId, EmployeeId = _employeeAccessor.EmployeeId })
            .MapAsync(specialty => new SpecialtyDto(specialty))
            .MatchAsync(specialty => Ok(specialty), error => error.MapToActionResult());
    }

    [HttpPost("specialty")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ChooseSpecialtyAsync([FromBody] ChooseSpecialtyRequest request)
    {
        return await _mediator
            .Send(request.ToCommand(_employeeAccessor.EmployeeId))
            .MatchAsync(() => Ok(), error => error.MapToActionResult());
    }
}
