using KpiV3.Domain.Comments.DataContracts;
using KpiV3.Domain.Comments.Ports;
using KpiV3.Domain.Employees.Ports;
using MediatR;

namespace KpiV3.Domain.Comments.Commands;

public record UpdateCommentCommand : IRequest<Result<CommentWithAuthor, IError>>
{
    public Guid CommentId { get; set; }
    public string Content { get; set; } = default!;
    public Guid IdOfWhoWantsToUpdate { get; set; }
}

public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, Result<CommentWithAuthor, IError>>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public UpdateCommentCommandHandler(
        ICommentRepository commentRepository, 
        IEmployeeRepository employeeRepository)
    {
        _commentRepository = commentRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<Result<CommentWithAuthor, IError>> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        return await _commentRepository
            .FindByIdAsync(request.CommentId)
            .BindAsync(comment => comment.CanBeModifiedBy(request.IdOfWhoWantsToUpdate))
            .MapAsync(comment => comment with { Content = request.Content })
            .BindAsync(comment => _commentRepository
                .UpdateAsync(comment)
                .InsertSuccessAsync(() => _employeeRepository
                    .FindByIdAsync(comment.AuthorId)
                    .MapAsync(author => new CommentWithAuthor(comment, author))));
    }
}
