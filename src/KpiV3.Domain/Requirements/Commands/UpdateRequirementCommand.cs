using KpiV3.Domain.Requirements.DataContracts;
using MediatR;

namespace KpiV3.Domain.Requirements.Commands;

public record UpdateRequirementCommand : IRequest<Requirement>
{
    public Guid RequirementId { get; init; }
    public string? Note { get; init; }
    public double Weight { get; init; }
}

public class UpdateRequirementCommandHandler : IRequestHandler<UpdateRequirementCommand, Requirement>
{
    private readonly KpiContext _db;

    public UpdateRequirementCommandHandler(KpiContext db)
    {
        _db = db;
    }

    public async Task<Requirement> Handle(UpdateRequirementCommand request, CancellationToken cancellationToken)
    {
        var requirement = await _db.Requirements
            .FindAsync(new object?[] { request.RequirementId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        requirement.Weight = request.Weight;
        requirement.Note = request.Note;

        await _db.SaveChangesAsync(cancellationToken);

        return requirement;
    }
}
