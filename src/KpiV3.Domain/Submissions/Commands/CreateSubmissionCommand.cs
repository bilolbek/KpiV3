using KpiV3.Domain.Common;
using KpiV3.Domain.Files.Ports;
using KpiV3.Domain.PeriodParts.Ports;
using KpiV3.Domain.Requirements.Ports;
using KpiV3.Domain.SpecialtyChoices.Ports;
using KpiV3.Domain.Submissions.DataContracts;
using KpiV3.Domain.Submissions.Repositories;
using MediatR;

namespace KpiV3.Domain.Submissions.Commands;

public record CreateSubmissionCommand : IRequest<Result<Submission, IError>>
{
    public Guid EmployeeId { get; set; }
    public Guid RequirementId { get; set; }
    public Guid FileId { get; set; }
    public string? Note { get; set; }
}

public class CreateSubmissionCommandHandler : IRequestHandler<CreateSubmissionCommand, Result<Submission, IError>>
{
    private readonly IFileMetadataRepository _fileMetadataRepository;
    private readonly IRequirementRepository _requirementRepository;
    private readonly ISubmissionRepository _submissionRepository;
    private readonly ISpecialtyChoiceRepository _specialtyChoiceRepository;
    private readonly IPeriodPartRepository _periodPartRepository;
    private readonly IGuidProvider _guidProvider;
    private readonly IDateProvider _dateProvider;

    public CreateSubmissionCommandHandler(
        IFileMetadataRepository fileMetadataRepository,
        IRequirementRepository requirementRepository,
        ISubmissionRepository submissionRepository,
        ISpecialtyChoiceRepository specialtyChoiceRepository,
        IPeriodPartRepository periodPartRepository,
        IGuidProvider guidProvider,
        IDateProvider dateProvider)
    {
        _fileMetadataRepository = fileMetadataRepository;
        _requirementRepository = requirementRepository;
        _submissionRepository = submissionRepository;
        _specialtyChoiceRepository = specialtyChoiceRepository;
        _periodPartRepository = periodPartRepository;
        _guidProvider = guidProvider;
        _dateProvider = dateProvider;
    }

    public async Task<Result<Submission, IError>> Handle(CreateSubmissionCommand request, CancellationToken cancellationToken)
    {
        return await EnsureEmployeeOwnsFileAsync(request.EmployeeId, request.FileId)
            .BindAsync(() => EnsureEmployeeCanSubmitRequirementAsync(request.EmployeeId, request.RequirementId))
            .InsertSuccessAsync(() => CreateSubmissionAsync(request));
    }

    private async Task<Result<Submission, IError>> CreateSubmissionAsync(CreateSubmissionCommand request)
    {
        var submission = new Submission
        {
            Id = _guidProvider.New(),

            FileId = request.FileId,
            Note = request.Note,
            RequirementId = request.RequirementId,
            UploaderId = request.EmployeeId,

            SubmissionDate = _dateProvider.Now(),
        };

        return await _submissionRepository
            .InsertAsync(submission)
            .InsertSuccessAsync(() => submission);
    }

    private async Task<Result<IError>> EnsureEmployeeCanSubmitRequirementAsync(Guid employeeId, Guid requirementId)
    {
        return await _requirementRepository
            .FindByIdAsync(requirementId)
            .BindAsync(requirement =>
                _periodPartRepository
                    .FindByIdAsync(requirement.PeriodPartId)
                    .BindAsync(part => _specialtyChoiceRepository.FindByEmployeeIdAndPeriodIdAsync(employeeId, part.PeriodId))
                    .BindAsync(specialty => requirement.SpecialtyId == specialty.SpecialtyId ?
                        Result<IError>.Ok() :
                        Result<IError>.Fail(new BusinessRuleViolation("You cannot add submission to this requirement because you do not have the required specialty."))));
    }

    private async Task<Result<IError>> EnsureEmployeeOwnsFileAsync(Guid employeeId, Guid fileId)
    {
        return await _fileMetadataRepository
            .FindByIdAsync(fileId)
            .BindAsync(metadata => metadata.UploaderId == employeeId ?
                Result<IError>.Ok() :
                Result<IError>.Fail(new ForbidenAction("File does not belong to employee")));
    }
}
