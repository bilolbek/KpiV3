using KpiV3.Domain.Comments.Commands;
using KpiV3.Domain.Comments.Queries;
using KpiV3.Domain.DataContracts.Models;
using KpiV3.WebApi.Authentication;
using KpiV3.WebApi.DataContracts.Comments;
using KpiV3.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[Authorize]
[ApiController]
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
    [ProducesResponseType(200, Type = typeof(Page<CommentDto>))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetAsync(Guid blockId, [FromQuery] GetCommentsRequest request)
    {
        return await _mediator
            .Send(request.ToQuery(blockId))
            .MapAsync(comments => comments.Map(comment => new CommentDto(comment)))
            .MatchAsync(comments => Ok(comments), error => error.MapToActionResult());
    }

    [HttpGet("{commentId:guid}")]
    [ProducesResponseType(200, Type = typeof(CommentDto))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByIdAsync(Guid commentId)
    {
        return await _mediator
            .Send(new GetCommentQuery { CommentId = commentId })
            .MapAsync(comment => new CommentDto(comment))
            .MatchAsync(comment => Ok(comment), error => error.MapToActionResult());
    }

    [HttpPost]
    [ProducesResponseType(200, Type = typeof(CommentDto))]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCommentRequest request)
    {
        return await _mediator
            .Send(request.ToCommand(_employeeAccessor.EmployeeId))
            .MapAsync(comment => new CommentDto(comment))
            .MatchAsync(comment => Ok(comment), error => error.MapToActionResult());
    }

    [HttpPut("{commentId:guid}")]
    [ProducesResponseType(200, Type = typeof(CommentDto))]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdateAsync(Guid commentId, [FromBody] UpdateCommentRequest request)
    {
        return await _mediator
            .Send(request.ToCommand(commentId, _employeeAccessor.EmployeeId))
            .MapAsync(comment => new CommentDto(comment))
            .MatchAsync(comment => Ok(comment), error => error.MapToActionResult());
    }

    [HttpDelete("{commentId:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteAsync(Guid commentId)
    {
        return await _mediator
            .Send(new DeleteCommentCommand { CommentId = commentId, IdOfWhoWantsToDelete = _employeeAccessor.EmployeeId })
            .MatchAsync(() => Ok(), error => error.MapToActionResult());
    }
}
