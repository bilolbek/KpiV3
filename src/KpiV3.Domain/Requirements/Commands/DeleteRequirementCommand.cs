using KpiV3.Domain.Requirements.Ports;
using MediatR;

namespace KpiV3.Domain.Requirements.Commands;

public record DeleteRequirementCommand : IRequest<Result<IError>>
{
    public Guid RequirementId { get; set; }
}

public class DeleteRequirementCommandHandler : IRequestHandler<DeleteRequirementCommand, Result<IError>>
{
    private readonly IRequirementRepository _repository;

    public DeleteRequirementCommandHandler(IRequirementRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IError>> Handle(DeleteRequirementCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.RequirementId);
    }
}
