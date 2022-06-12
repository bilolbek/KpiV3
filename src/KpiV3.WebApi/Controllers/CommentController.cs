using KpiV3.Domain.Comments.Commands;
using KpiV3.Domain.Comments.Queries;
using KpiV3.WebApi.Authentication.Services;
using KpiV3.WebApi.DataContracts.Comments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IEmployeeAccessor _employeeAccessor;

    public CommentController(
        IMediator mediator,
        IEmployeeAccessor employeeAccessor)
    {
        _mediator = mediator;
        _employeeAccessor = employeeAccessor;
    }

    [HttpGet("block/{blockId:guid}")]
    public async Task<IActionResult> GetAsync(Guid blockId, [FromQuery] GetCommentsRequest request, CancellationToken cancellationToken)
    {
        var comments = await _mediator.Send(request.ToQuery(blockId), cancellationToken);

        return Ok(comments.Map(c => new CommentDto(c)));
    }

    [HttpGet("{commentId:guid}")]
    public async Task<IActionResult> GetAsync(Guid commentId, CancellationToken cancellationToken)
    {
        var comment = await _mediator.Send(new GetCommentQuery { CommentId = commentId }, cancellationToken);

        return Ok(new CommentDto(comment));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCommentRequest request)
    {
        var comment = await _mediator.Send(request.ToCommand(_employeeAccessor.EmployeeId));

        return Ok(new CommentDto(comment));
    }

    [HttpPut("{commentId:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid commentId, [FromBody] UpdateCommentRequest request)
    {
        var comment = await _mediator.Send(request.ToCommand(commentId, _employeeAccessor.PositionId));

        return Ok(new CommentDto(comment));
    }

    [HttpDelete("{commentId:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid commentId)
    {
        await _mediator.Send(new DeleteCommentCommand
        {
            IdOfWhoWantsToDelete = _employeeAccessor.EmployeeId,
            CommentId = commentId,
        });

        return Ok();
    }
}
