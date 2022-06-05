using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Employees.Commands;
using KpiV3.Domain.SpecialtyChoices.Commands;
using KpiV3.WebApi.Converters;
using KpiV3.WebApi.DataContracts.Employees;
using KpiV3.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[Authorize(Policy = "RootOnly")]
[ApiController]
[Route("api/[controller]")]
[ApiVersion("3.0")]
public class EmployeeController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(Page<ProfileDto>))]
    public async Task<IActionResult> GetAsync([FromQuery] GetEmployeesRequest request)
    {
        return await _mediator
            .Send(request.ToQuery())
            .MapAsync(x => x.Map(x => new ProfileDto(x)))
            .MatchAsync(x => Ok(x), error => error.MapToActionResult());
    }

    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterEmployeeRequest request)
    {
        return await _mediator
            .Send(request.ToCommand())
            .MatchAsync(() => Ok(), error => error.MapToActionResult());
    }

    [HttpPost("import")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> ImportAsync([FromForm] IFormFile file)
    {
        return await CsvConverter
            .Convert<CsvImportedEmployee>(file.OpenReadStream())
            .Map(employees => employees.Select(e => e.ToRegisterEmployee()).ToList())
            .Map(employees => new ImportEmployeesCommand { Employees = employees })
            .BindAsync(command => _mediator.Send(command))
            .MatchAsync(() => Ok(), error => error.MapToActionResult());
    }

    [HttpPost("reset-password/{employeeId:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ResetPasswordAsync(Guid employeeId)
    {
        return await _mediator
            .Send(new ResetPasswordCommand { EmployeeId = employeeId })
            .MatchAsync(() => Ok(), error => error.MapToActionResult());
    }

    [HttpPost("allow-specialty-change")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> AllowSpecialtyChangeAsync([FromBody] AllowSpecialtyChangeRequest request)
    {
        return await _mediator
            .Send(request.ToCommand())
            .MatchAsync(() => Ok(), error => error.MapToActionResult());
    }
}
