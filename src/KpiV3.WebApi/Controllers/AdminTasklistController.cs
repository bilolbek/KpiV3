using KpiV3.Domain.Tasklists.Queries;
using KpiV3.WebApi.DataContracts.Tasklists;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[ApiController]
[Authorize(Policy = "RootOnly")]
[Route("api/[controller]")]
public class AdminTasklistController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminTasklistController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] GetAdminTasklistRequest request)
    {
        return Ok(await _mediator.Send(request.ToQuery()));
    }

    [HttpGet("{employeeId:guid}/{periodId:guid}")]
    public async Task<IActionResult> GetEmployeeTasksAsync(Guid employeeId, Guid periodId)
    {
        return Ok(await _mediator.Send(new GetEmployeeTasksQuery
        {
            EmployeeId = employeeId,
            PeriodId = periodId,
        }));
    }
}
