using MediatR;

namespace KpiV3.Domain.Grades.Commands;

public record DeleteGradeCommand : IRequest
{
    public Guid RequirementId { get; init; }
    public Guid EmployeeId { get; init; }
}

public class DeleteGradeCommandHandler : AsyncRequestHandler<DeleteGradeCommand>
{
    private readonly KpiContext _db;

    public DeleteGradeCommandHandler(KpiContext db)
    {
        _db = db;
    }

    protected override async Task Handle(DeleteGradeCommand request, CancellationToken cancellationToken)
    {
        var grade = await _db.Grades
            .FirstOrDefaultAsync(g =>
                g.RequirementId == request.RequirementId &&
                g.EmployeeId == request.EmployeeId, cancellationToken)
            .EnsureFoundAsync();

        _db.Remove(grade);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
