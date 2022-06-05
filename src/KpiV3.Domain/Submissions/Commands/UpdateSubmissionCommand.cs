using KpiV3.Domain.Files.Ports;
using KpiV3.Domain.Submissions.DataContracts;
using KpiV3.Domain.Submissions.Repositories;
using MediatR;

namespace KpiV3.Domain.Submissions.Commands;

public record UpdateSubmissionCommand : IRequest<Result<Submission, IError>>
{
    public Guid SubmissionId { get; set; }
    public Guid FileId { get; set; }
    public string? Note { get; set; }
    public Guid IdOfWhoWantsToUpdate { get; set; }
}

public class UpdateSubmissionCommandHandler : IRequestHandler<UpdateSubmissionCommand, Result<Submission, IError>>
{
    private readonly IFileMetadataRepository _fileMetadataRepository;
    private readonly ISubmissionRepository _submissionRepository;

    public UpdateSubmissionCommandHandler(
        IFileMetadataRepository fileMetadataRepository,
        ISubmissionRepository submissionRepository)
    {
        _fileMetadataRepository = fileMetadataRepository;
        _submissionRepository = submissionRepository;
    }

    public async Task<Result<Submission, IError>> Handle(UpdateSubmissionCommand request, CancellationToken cancellationToken)
    {
        return await _submissionRepository
            .FindByIdAsync(request.SubmissionId)
            .BindAsync(submission => EnsureSubmissionBelongsToEmployee(submission, request.IdOfWhoWantsToUpdate)
                .BindAsync(() => EnsureEmployeeOwnsFileAsync(submission.UploaderId, request.FileId))
                .InsertSuccessAsync(() => submission))
            .MapAsync(submission => submission with
            {
                Note = request.Note,
                FileId = request.FileId,
            })
            .BindAsync(submission => _submissionRepository
                .UpdateAsync(submission)
                .InsertSuccessAsync(() => submission));
    }

    private static Result<IError> EnsureSubmissionBelongsToEmployee(Submission submission, Guid employeeId)
    {
        if (submission.UploaderId != employeeId)
        {
            return Result<IError>.Fail(new ForbidenAction("This submission does not belong to you"));
        }

        return Result<IError>.Ok();
    }

    private async Task<Result<IError>> EnsureEmployeeOwnsFileAsync(Guid employeeId, Guid fileId)
    {
        return await _fileMetadataRepository
            .FindByIdAsync(fileId)
            .BindAsync(metadata => metadata.UploaderId == employeeId
                ? Result<IError>.Ok()
                : Result<IError>.Fail(new ForbidenAction("File does not belong yo you")));
    }
}
