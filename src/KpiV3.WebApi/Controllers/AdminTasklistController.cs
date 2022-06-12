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
    public async Task<IActionResult> GetAsync(GetAdminTasklistRequest request)
    {
        return Ok(await _mediator.Send(request.ToQuery()));
    }
}
