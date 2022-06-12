using KpiV3.Domain.Common.DataContracts;
using KpiV3.Domain.Employees.Commands;
using KpiV3.Domain.Employees.Queries;
using KpiV3.WebApi.DataContracts.Common;
using KpiV3.WebApi.DataContracts.Employees;
using KpiV3.WebApi.Misc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[Authorize(Policy = "RootOnly")]
[ApiController]
[Route("api/[controller]")]
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
        var employees = await _mediator.Send(request.ToQuery());

        return Ok(employees.Map(p => new ProfileDto(p)));
    }

    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterEmployeeRequest request)
    {
        await _mediator.Send(request.ToCommand());

        return Ok();
    }

    [HttpPost("import")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> ImportAsync([FromForm] IFormFile file)
    {
        await _mediator.Send(new ImportEmployeesCommand
        {
            Employees = CsvConverter
                .Convert<CsvImportedEmployee>(file.OpenReadStream())
                .Select(e => e.ToRegisterEmployee())
                .ToList()
        });

        return Ok();
    }

    [HttpPost("reset-password/{employeeId:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ResetPasswordAsync(Guid employeeId)
    {
        await _mediator.Send(new ResetPasswordCommand { EmployeeId = employeeId });

        return Ok();
    }

    [HttpPost("allow-specialty-change")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> AllowSpecialtyChangeAsync([FromBody] AllowSpecialtyChangeRequest request)
    {
        await _mediator.Send(request.ToCommand());

        return Ok();
    }
}