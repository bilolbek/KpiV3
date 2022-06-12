using KpiV3.Domain.Files.Services;
using KpiV3.Domain.Submissions.DataContracts;
using MediatR;

namespace KpiV3.Domain.Submissions.Commands;

public record UpdateSubmissionCommand : IRequest<Submission>
{
    public Guid SubmissionId { get; init; }
    public Guid EmployeeId { get; init; }
    public List<Guid> FileIds { get; init; } = default!;
}

public class UpdateSubmissionCommandHandler : IRequestHandler<UpdateSubmissionCommand, Submission>
{
    private readonly KpiContext _db;
    private readonly FileOwnershipService _fileOwnershipService;

    public UpdateSubmissionCommandHandler(
        KpiContext db,
        FileOwnershipService fileOwnershipService)
    {
        _db = db;
        _fileOwnershipService = fileOwnershipService;
    }

    public async Task<Submission> Handle(UpdateSubmissionCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);

        var submission = await _db.Submissions
            .Include(s => s.Requirement)
            .Include(s => s.Files)
            .FirstOrDefaultAsync(s => s.Id == request.SubmissionId, cancellationToken)
            .EnsureFoundAsync();

        submission.Files.Clear();
        request.FileIds.ForEach(fileId => submission.Files.Add(new SubmissionFile { FileId = fileId }));

        EnsureEmployeeOwnsSubmissionAsync(submission, request);
        await _fileOwnershipService.EnsureEmployeeOwnsFilesAsync(request.EmployeeId, request.FileIds, cancellationToken);

        await _db.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return submission;
    }

    private static void EnsureEmployeeOwnsSubmissionAsync(Submission submission, UpdateSubmissionCommand request)
    {
        if (submission.EmployeeId != request.EmployeeId)
        {
            throw new ForbiddenActionException("This is not your submission");
        }
    }
}
