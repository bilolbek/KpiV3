using KpiV3.WebApi.DataContracts.Positions;
using KpiV3.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("3.0")]
public class PositionController : ControllerBase
{
    private readonly IMediator _mediator;

    public PositionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(200, Type = typeof(CreatePositionResponse))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreatePositionRequest request)
    {
        return await _mediator
            .Send(request.ToCommand())
            .MapAsync(position => new CreatePositionResponse(position))
            .MatchAsync(response => Ok(response), error => error.MapToActionResult());
    }
}
