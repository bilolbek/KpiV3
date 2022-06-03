using KpiV3.WebApi.DataContracts.Employees;
using KpiV3.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[Authorize(Policy = "RootOnly")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("3.0")]
public class EmployeeController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeeController(IMediator mediator)
    {
        _mediator = mediator;
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
}
