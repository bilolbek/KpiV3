using KpiV3.Domain.Posts.Commands;
using KpiV3.Domain.Posts.Queries;
using KpiV3.WebApi.Authentication.Services;
using KpiV3.WebApi.DataContracts.Posts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KpiV3.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
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
    public async Task<IActionResult> GetAsync([FromQuery] GetPostsRequest request, CancellationToken cancellationToken)
    {
        var posts = await _mediator.Send(request.ToQuery(), cancellationToken);

        return Ok(posts.Map(p => new PostDto(p)));
    }

    [HttpGet("{postId:guid}")]
    public async Task<IActionResult> GetAsync(Guid postId, CancellationToken cancellationToken)
    {
        var post = await _mediator.Send(new GetPostQuery { PostId = postId }, cancellationToken);

        return Ok(new PostDto(post));
    }

    [Authorize(Policy = "RootOnly")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreatePostRequest request)
    {
        var post = await _mediator.Send(request.ToCommand(_employeeAccessor.EmployeeId));

        return Ok(new PostDto(post));
    }

    [Authorize(Policy = "RootOnly")]
    [HttpPut("{postId:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid postId, [FromBody] UpdatePostRequest request)
    {
        var post = await _mediator.Send(request.ToCommand(postId));

        return Ok(new PostDto(post));
    }

    [Authorize(Policy = "RootOnly")]
    [HttpDelete("{postId:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid postId)
    {
        await _mediator.Send(new DeletePostCommand { PostId = postId });
        return Ok();
    }
}
