using KpiV3.Domain.Comments.Ports;
using MediatR;

namespace KpiV3.Domain.Comments.Commands;

public record DeleteCommentCommand : IRequest<Result<IError>>
{
    public Guid CommentId { get; set; }
    public Guid IdOfWhoWantsToDelete { get; set; }
}

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Result<IError>>
{
    private readonly ICommentRepository _repository;

    public DeleteCommentCommandHandler(ICommentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IError>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        return await _repository
            .FindByIdAsync(request.CommentId)
            .BindAsync(comment => comment.CanBeModifiedBy(request.IdOfWhoWantsToDelete))
            .BindAsync(comment => _repository.DeleteAsync(comment.Id));
    }
}