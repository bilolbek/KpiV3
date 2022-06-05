using KpiV3.Domain.Employees.Queries;
using KpiV3.WebApi.Authentication;
using KpiV3.WebApi.DataContracts.Employees;
using KpiV3.WebApi.DataContracts.Profiles;
using KpiV3.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
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
}
