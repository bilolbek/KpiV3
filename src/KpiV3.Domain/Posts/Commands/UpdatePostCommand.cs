using KpiV3.Domain.Employees.Ports;
using KpiV3.Domain.Posts.DataContracts;
using KpiV3.Domain.Posts.Ports;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KpiV3.Domain.Posts.Commands;

public record UpdatePostCommand : IRequest<Result<PostWithAuthor, IError>>
{
    public Guid PostId { get; set; }

    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;

    public Guid IdOfWhoWantsToEdit { get; set; }
}


public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, Result<PostWithAuthor, IError>>
{
    private readonly IPostRepository _postRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public UpdatePostCommandHandler(
        IPostRepository postRepository, 
        IEmployeeRepository employeeRepository)
    {
        _postRepository = postRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<Result<PostWithAuthor, IError>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        return await _postRepository
            .FindByIdAsync(request.PostId)
            .BindAsync(post => post.CanBeModifiedBy(request.IdOfWhoWantsToEdit))
            .MapAsync(p => p with { Title = request.Title, Content = request.Content })
            .BindAsync(post => _postRepository
                .UpdateAsync(post)
                .InsertSuccessAsync(() => _employeeRepository
                    .FindByIdAsync(post.AuthorId)
                    .MapAsync(author => new PostWithAuthor(post, author))));
    }
}