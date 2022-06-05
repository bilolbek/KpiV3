using KpiV3.Domain.Submissions.Repositories;
using MediatR;

namespace KpiV3.Domain.Submissions.Commands;

public record DeleteSubmissionCommand : IRequest<Result<IError>>
{
    public Guid SubmissionId { get; set; }
    public Guid IdOfWhoWantsToDelete { get; set; }
}

public class DeleteSubmissionCommandHandler : IRequestHandler<DeleteSubmissionCommand, Result<IError>>
{
    private readonly ISubmissionRepository _repository;

    public DeleteSubmissionCommandHandler(ISubmissionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IError>> Handle(DeleteSubmissionCommand request, CancellationToken cancellationToken)
    {
        return await _repository
            .FindByIdAsync(request.SubmissionId)
            .BindAsync(submission => submission.UploaderId != request.IdOfWhoWantsToDelete ?
                Result<IError>.Fail(new ForbidenAction("This submission does not belong to you")) :
                Result<IError>.Ok())
            .BindAsync(() => _repository.DeleteAsync(request.SubmissionId));
    }
}
