using KpiV3.Domain.Requirements.Commands;
using KpiV3.Domain.Requirements.Queries;
using KpiV3.WebApi.DataContracts.Requirements;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RequirementController : ControllerBase
{
    private readonly IMediator _mediator;

    public RequirementController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{requirementId:guid}")]
    public async Task<IActionResult> GetAsync(Guid requirementId)
    {
        var requirement = await _mediator.Send(new GetRequirementQuery { RequirementId = requirementId });

        return Ok(new RequirementDto(requirement));
    }

    [HttpGet("of-specialty/{specialtyId:guid}/{periodId:guid}")]
    public async Task<IActionResult> GetRequirementsOfSpecialtyAsync(Guid specialtyId, Guid periodId)
    {
        var requirements = await _mediator.Send(new GetRequirementsOfSpecialtyQuery
        {
            PeriodId = periodId,
            SpecialtyId = specialtyId,
        });

        return Ok(requirements);
    }

    [HttpPost]
    [Authorize(Policy = "RootOnly")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateRequirementRequest request)
    {
        var requirement = await _mediator.Send(request.ToCommand());

        return Ok(new RequirementDto(requirement));
    }

    [HttpPut("{requirementId:guid}")]
    [Authorize(Policy = "RootOnly")]
    public async Task<IActionResult> UpdateAsync(Guid requirementId, [FromBody] UpdateRequirementRequest request)
    {
        var requirement = await _mediator.Send(request.ToCommand(requirementId));

        return Ok(new RequirementDto(requirement));
    }

    [HttpDelete("{requirementId:guid}")]
    [Authorize(Policy = "RootOnly")]
    public async Task<IActionResult> DeleteAsync(Guid requirementId)
    {
        await _mediator.Send(new DeleteRequirementCommand { RequirementId = requirementId });

        return Ok();
    }
}
