using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Posts.Commands;
using KpiV3.Domain.Posts.Queries;
using KpiV3.WebApi.Authentication;
using KpiV3.WebApi.DataContracts.Posts;
using KpiV3.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("3.0")]
public class PostController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IEmployeeAccessor _employeeAccessor;

    public PostController(
        IMediator mediator,
        IEmployeeAccessor employeeAccessor)
    {
        _mediator = mediator;
        _employeeAccessor = employeeAccessor;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(Page<PostDto>))]
    public async Task<IActionResult> GetAsync([FromQuery] GetPostsRequest request)
    {
        return await _mediator
            .Send(request.ToQuery())
            .MapAsync(posts => posts.Map(post => new PostDto(post)))
            .MatchAsync(posts => Ok(posts), error => error.MapToActionResult());
    }

    [HttpGet("{postId:guid}")]
    [ProducesResponseType(200, Type = typeof(PostDto))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByIdAsync(Guid postId)
    {
        return await _mediator
            .Send(new GetPostQuery { PostId = postId })
            .MapAsync(post => new PostDto(post))
            .MatchAsync(post => Ok(post), error => error.MapToActionResult());
    }

    [HttpPost]
    [Authorize(Policy = "RootOnly")]
    [ProducesResponseType(200, Type = typeof(PostDto))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateAsync([FromBody] CreatePostRequest request)
    {
        return await _mediator
            .Send(request.ToCommand(_employeeAccessor.EmployeeId))
            .MapAsync(post => new PostDto(post))
            .MatchAsync(post => Ok(post), error => error.MapToActionResult());
    }

    [HttpPut("{postId:guid}")]
    [Authorize(Policy = "RootOnly")]
    [ProducesResponseType(200, Type = typeof(PostDto))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdateAsync(Guid postId, [FromBody] UpdatePostRequest request)
    {
        return await _mediator
            .Send(request.ToCommand(postId, _employeeAccessor.EmployeeId))
            .MapAsync(post => new PostDto(post))
            .MatchAsync(post => Ok(post), error => error.MapToActionResult());
    }

    [HttpDelete("{postId:guid}")]
    [Authorize(Policy = "RootOnly")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteAsync(Guid postId)
    {
        return await _mediator
            .Send(new DeletePostCommand { PostId = postId, IdOfWhoWantsToDelete = _employeeAccessor.EmployeeId })
            .MatchAsync(() => Ok(), error => error.MapToActionResult());
    }
}
