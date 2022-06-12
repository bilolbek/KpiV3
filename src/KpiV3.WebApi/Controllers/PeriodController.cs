using KpiV3.Domain.Common.DataContracts;
using KpiV3.Domain.Periods.Commands;
using KpiV3.Domain.Periods.Queries;
using KpiV3.WebApi.DataContracts.Periods;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PeriodController : ControllerBase
{
    private readonly IMediator _mediator;

    public PeriodController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(Page<PeriodDto>))]
    public async Task<IActionResult> GetAsync([FromQuery] GetPeriodsRequest request, CancellationToken cancellationToken)
    {
        var periods = await _mediator.Send(request.ToQuery(), cancellationToken);

        return Ok(periods.Map(p => new PeriodDto(p)));
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveAsync()
    {
        var period = await _mediator.Send(new GetActivePeriodQuery());

        return Ok(new PeriodDto(period));
    }

    [HttpGet("{period:guid}")]
    [ProducesResponseType(200, Type = typeof(PeriodDto))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetAsync(Guid periodId, CancellationToken cancellationToken)
    {
        var period = await _mediator.Send(new GetPeriodQuery { PeriodId = periodId }, cancellationToken);

        return Ok(new PeriodDto(period));
    }

    [HttpPost]
    [Authorize(Policy = "RootOnly")]
    [ProducesResponseType(200, Type = typeof(PeriodDto))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateAsync([FromBody] CreatePeriodRequest request)
    {
        var period = await _mediator.Send(request.ToCommand());

        return Ok(new PeriodDto(period));
    }

    [HttpPut("{periodId:guid}")]
    [Authorize(Policy = "RootOnly")]
    [ProducesResponseType(200, Type = typeof(PeriodDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateAsync(Guid periodId, [FromBody] UpdatePeriodRequest request)
    {
        var period = await _mediator.Send(request.ToCommand(periodId));

        return Ok(period);
    }

    [HttpDelete("{periodId:guid}")]
    [Authorize(Policy = "RootOnly")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteAsync(Guid periodId)
    {
        await _mediator.Send(new DeletePeriodCommand
        {
            PeriodId = periodId
        });

        return Ok();
    }
}
