using KpiV3.Domain.Comments.DataContracts;
using KpiV3.Domain.Files.Services;
using KpiV3.Domain.Submissions.DataContracts;
using MediatR;

namespace KpiV3.Domain.Submissions.Commands;

public record CreateSubmissionCommand : IRequest<Submission>
{
    public List<Guid> FileIds { get; init; } = default!;
    public Guid RequirementId { get; init; }
    public Guid EmployeeId { get; init; }
}

public class CreateSubmissionCommandHandler : IRequestHandler<CreateSubmissionCommand, Submission>
{
    private readonly KpiContext _db;
    private readonly IGuidProvider _guidProvider;
    private readonly IDateProvider _dateProvider;
    private readonly FileOwnershipService _fileOwnershipService;

    public CreateSubmissionCommandHandler(
        KpiContext db,
        IGuidProvider guidProvider,
        IDateProvider dateProvider,
        FileOwnershipService fileOwnershipService)
    {
        _db = db;
        _guidProvider = guidProvider;
        _dateProvider = dateProvider;
        _fileOwnershipService = fileOwnershipService;
    }

    public async Task<Submission> Handle(CreateSubmissionCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);

        await PerformValidationsAsync(request, cancellationToken);

        var submission = new Submission
        {
            Id = _guidProvider.New(),
            EmployeeId = request.EmployeeId,
            RequirementId = request.RequirementId,
            SubmittedDate = _dateProvider.Now(),
            Files = request.FileIds.Select(fileId => new SubmissionFile
            {
                FileId = fileId,
            }).ToList(),
            CommentBlock = new CommentBlock
            {
                Id = _guidProvider.New(),
                Type = CommentBlockType.Submission,
            },
        };

        _db.Submissions.Add(submission);

        await _db.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return submission;
    }

    private async Task PerformValidationsAsync(CreateSubmissionCommand request, CancellationToken cancellationToken)
    {
        await _fileOwnershipService.EnsureEmployeeOwnsFilesAsync(request.EmployeeId, request.FileIds, cancellationToken);
        await EnsureEmployeeCanSubmitToRequirementAsync(request, cancellationToken);
    }

    private async Task EnsureEmployeeCanSubmitToRequirementAsync(
        CreateSubmissionCommand request,
        CancellationToken cancellationToken)
    {
        var requirement = await _db.Requirements
            .Include(r => r.PeriodPart)
            .FirstOrDefaultAsync(r => r.Id == request.RequirementId, cancellationToken)
            .EnsureFoundAsync();

        var specialtyOfEmployee = await _db.SpecialtyChoices
            .FirstOrDefaultAsync(sc =>
                sc.EmployeeId == request.EmployeeId &&
                sc.PeriodId == requirement.PeriodPart.PeriodId, cancellationToken);


        if (specialtyOfEmployee is null || specialtyOfEmployee.SpecialtyId != requirement.SpecialtyId)
        {
            throw new ForbiddenActionException("You cannot submit to this requirement");
        }
    }
}
