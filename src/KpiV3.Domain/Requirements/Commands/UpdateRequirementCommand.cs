using KpiV3.Domain.Requirements.DataContracts;
using KpiV3.Domain.Requirements.Ports;
using MediatR;

namespace KpiV3.Domain.Requirements.Commands;

public record UpdateRequirementCommand : IRequest<Result<Requirement, IError>>
{
    public Guid RequirementId { get; set; }
    public string? Note { get; set; }
    public double Weight { get; set; }
}

public class UpdateRequirementCommandHandler : IRequestHandler<UpdateRequirementCommand, Result<Requirement, IError>>
{
    private readonly IRequirementRepository _repository;

    public UpdateRequirementCommandHandler(IRequirementRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Requirement, IError>> Handle(UpdateRequirementCommand request, CancellationToken cancellationToken)
    {
        return await _repository
            .FindByIdAsync(request.RequirementId)
            .MapAsync(requirement => requirement with { Note = request.Note, Weight = request.Weight })
            .BindAsync(requirement => _repository
                .UpdateAsync(requirement)
                .InsertSuccessAsync(() => requirement));
    }
}
