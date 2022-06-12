using KpiV3.Domain.Employees.Services;
using MediatR;

namespace KpiV3.Domain.Submissions.Commands;

public record DeleteSubmissionCommand : IRequest
{
    public Guid SubmissionId { get; init; }
    public Guid IdOfWhoWantsToDelete { get; init; }
}

public class DeleteSubmissionCommandHandler : AsyncRequestHandler<DeleteSubmissionCommand>
{
    private readonly KpiContext _db;
    private readonly EmployeePositionService _employeePositionService;

    public DeleteSubmissionCommandHandler(
        KpiContext db,
        EmployeePositionService employeePositionService)
    {
        _db = db;
        _employeePositionService = employeePositionService;
    }

    protected override async Task Handle(DeleteSubmissionCommand request, CancellationToken cancellationToken)
    {
        var submission = await _db.Submissions
            .FindAsync(new object?[] { request.SubmissionId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        if (submission.EmployeeId == request.IdOfWhoWantsToDelete ||
            await _employeePositionService.EmployeeHasRootPositionAsync(request.IdOfWhoWantsToDelete, cancellationToken))
        {
            _db.Submissions.Remove(submission);
        }
        else
        {
            throw new ForbiddenActionException("You don't have access to this submisison");
        }

        await _db.SaveChangesAsync(cancellationToken);
    }
}
