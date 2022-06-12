using KpiV3.Domain.Specialties.Commands;
using KpiV3.Domain.Specialties.Queries;
using KpiV3.WebApi.DataContracts.Specialties;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class SpecialtyController : ControllerBase
{
    private readonly IMediator _mediator;

    public SpecialtyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("by-position/{positionId:guid}")]
    [ProducesResponseType(200, Type = typeof(List<SpecialtyDto>))]
    public async Task<IActionResult> GetByPositionIdAsync(Guid positionId, CancellationToken cancellationToken)
    {
        var specialties = await _mediator.Send(new GetSpecialtiesQuery { PositionId = positionId }, cancellationToken);

        return Ok(specialties.Select(p => new SpecialtyDto(p)));
    }

    [HttpGet("{specialtyId:guid}")]
    [ProducesResponseType(200, Type = typeof(SpecialtyDto))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetAsync(Guid specialtyId, CancellationToken cancellationToken)
    {
        var specialty = await _mediator.Send(new GetSpecialtyQuery { SpecialtyId = specialtyId }, cancellationToken);

        return Ok(new SpecialtyDto(specialty));
    }

    [Authorize(Policy = "RootOnly")]
    [HttpPost]
    [ProducesResponseType(200, Type = typeof(SpecialtyDto))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateSpecialtyRequest request)
    {
        var specialty = await _mediator.Send(request.ToCommand());

        return Ok(new SpecialtyDto(specialty));
    }

    [Authorize(Policy = "RootOnly")]
    [HttpPut("{specialtyId:guid}")]
    [ProducesResponseType(200, Type = typeof(SpecialtyDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateAsync(Guid specialtyId, UpdateSpecialtyRequest request)
    {
        var specialty = await _mediator.Send(request.ToCommand(specialtyId));

        return Ok(new SpecialtyDto(specialty));
    }

    [Authorize(Policy = "RootOnly")]
    [HttpDelete("{specialtyId:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> DeleteAsync(Guid specialtyId)
    {
        await _mediator.Send(new DeleteSpecialtyCommand { SpecialtyId = specialtyId });

        return Ok();
    }
}
