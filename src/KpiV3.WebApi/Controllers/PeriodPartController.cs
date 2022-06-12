using Amazon.Auth.AccessControlPolicy;
using KpiV3.Domain.PeriodParts.Commands;
using KpiV3.Domain.PeriodParts.Queries;
using KpiV3.WebApi.DataContracts.PeriodParts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class PeriodPartController : ControllerBase
{
    private readonly IMediator _mediator;

    public PeriodPartController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("by-period/{periodId:guid}")]
    [ProducesResponseType(200, Type = typeof(List<PeriodPartDto>))]
    public async Task<IActionResult> GetByPeriodIdAsync(Guid periodId)
    {
        var parts = await _mediator.Send(new GetPeriodPartsQuery { PeriodId = periodId });

        return Ok(parts.Select(p => new PeriodPartDto(p)));
    }

    [HttpGet("{partId:guid}")]
    [ProducesResponseType(200, Type = typeof(PeriodPartDto))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByIdAsync(Guid partId)
    {
        var part = await _mediator.Send(new GetPeriodPartQuery { PeriodPartId = partId });

        return Ok(new PeriodPartDto(part));
    }

    [Authorize(Policy = "RootOnly")]
    [HttpPost]
    [ProducesResponseType(200, Type = typeof(PeriodPartDto))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateAsync([FromBody] CreatePeriodPartRequest request)
    {
        var part = await _mediator.Send(request.ToCommand());

        return Ok(new PeriodPartDto(part));
    }

    [Authorize(Policy = "RootOnly")]
    [HttpPut("{partId:guid}")]
    [ProducesResponseType(200, Type = typeof(PeriodPartDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateAsync(Guid partId, [FromBody] UpdatePeriodPartRequest request)
    {
        var part = await _mediator.Send(request.ToCommand(partId));

        return Ok(new PeriodPartDto(part));
    }

    [Authorize(Policy = "RootOnly")]
    [HttpDelete("{partId:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteAsync(Guid partId)
    {
        await _mediator.Send(new DeletePeriodPartCommand { PartId = partId });

        return Ok();
    }
}