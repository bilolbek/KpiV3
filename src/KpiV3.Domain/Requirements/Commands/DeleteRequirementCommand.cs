using MediatR;

namespace KpiV3.Domain.Requirements.Commands;

public record DeleteRequirementCommand : IRequest
{
    public Guid RequirementId { get; init; }
}

public class DeleteRequirementCommandHandler : AsyncRequestHandler<DeleteRequirementCommand>
{
    private readonly KpiContext _db;

    public DeleteRequirementCommandHandler(KpiContext db)
    {
        _db = db;
    }

    protected override async Task Handle(DeleteRequirementCommand request, CancellationToken cancellationToken)
    {
        var requirement = await _db
            .Requirements
            .FindAsync(new object?[] { request.RequirementId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        _db.Requirements.Remove(requirement);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
