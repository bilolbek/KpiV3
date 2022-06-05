using KpiV3.Domain.Posts.Ports;
using MediatR;

namespace KpiV3.Domain.Posts.Commands;

public record DeletePostCommand : IRequest<Result<IError>>
{
    public Guid PostId { get; set; }
    public Guid IdOfWhoWantsToDelete { get; set; }
}

public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, Result<IError>>
{
    private readonly IPostRepository _repository;

    public DeletePostCommandHandler(IPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IError>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        return await _repository
            .FindByIdAsync(request.PostId)
            .BindAsync(post => post.CanBeModifiedBy(request.IdOfWhoWantsToDelete))
            .BindAsync(post => _repository.DeleteAsync(post.Id));
    }
}
