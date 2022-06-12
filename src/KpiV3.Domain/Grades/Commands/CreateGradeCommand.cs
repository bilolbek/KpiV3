using KpiV3.Domain.Grades.DataContracts;
using KpiV3.Domain.Grades.Services;
using MediatR;

namespace KpiV3.Domain.Grades.Commands;

public record CreateGradeCommand : IRequest
{
    public Guid EmployeeId { get; init; }
    public Guid RequirementId { get; init; }
    public double Value { get; init; }
}

public class CreateGradeCommandHandler : AsyncRequestHandler<CreateGradeCommand>
{
    private readonly KpiContext _db;
    private readonly IDateProvider _dateProvider;
    private readonly GradeValidationService _gradeValidationService;

    public CreateGradeCommandHandler(
        KpiContext db,
        IDateProvider dateProvider,
        GradeValidationService gradeValidationService)
    {
        _db = db;
        _dateProvider = dateProvider;
        _gradeValidationService = gradeValidationService;
    }

    protected override async Task Handle(CreateGradeCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);

        var grade = new Grade
        {
            EmployeeId = request.EmployeeId,
            RequirementId = request.RequirementId,
            Value = request.Value,
            GradedDate = _dateProvider.Now(),
        };

        await _gradeValidationService.ValidateGradeAsync(grade, cancellationToken);

        _db.Grades.Add(grade);

        await transaction.CommitAsync(cancellationToken);
    }
}
