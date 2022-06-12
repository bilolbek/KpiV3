using KpiV3.Domain.Grades.Commands;
using KpiV3.WebApi.DataContracts.Grades;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "RootOnly")]
public class GradeController : ControllerBase
{
    private readonly IMediator _mediator;

    public GradeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{employeeId:guid}/{requirementId:guid}")]
    public async Task<IActionResult> PutAsync(Guid employeeId, Guid requirementId, [FromBody] GradeValue gradeValue)
    {
        await _mediator.Send(new PutGradeCommand
        {
            Value = gradeValue.Value,
            EmployeeId = employeeId,
            RequirementId = requirementId,
        });

        return Ok();
    }

    [HttpDelete("{employeeId:guid}/{requirementId:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid employeeId, Guid requirementId)
    {
        await _mediator.Send(new DeleteGradeCommand
        {
            EmployeeId = employeeId,
            RequirementId = requirementId,
        });

        return Ok();
    }
}
