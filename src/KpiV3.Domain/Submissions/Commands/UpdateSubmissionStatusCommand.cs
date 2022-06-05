using KpiV3.Domain.Submissions.DataContracts;
using KpiV3.Domain.Submissions.Repositories;
using MediatR;

namespace KpiV3.Domain.Submissions.Commands;

public record UpdateSubmissionStatusCommand : IRequest<Result<IError>>
{
    public Guid SubmissionId { get; set; }
    public SubmissionStatus Status { get; set; }
}

public class UpdateSubmissionStatusCommandHandler : IRequestHandler<UpdateSubmissionStatusCommand, Result<IError>>
{
    private readonly ISubmissionRepository _repository;

    public UpdateSubmissionStatusCommandHandler(ISubmissionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IError>> Handle(UpdateSubmissionStatusCommand request, CancellationToken cancellationToken)
    {
        return await _repository
            .FindByIdAsync(request.SubmissionId)
            .MapAsync(s => s with { Status = request.Status })
            .BindAsync(s => _repository.UpdateAsync(s));
    }
}
