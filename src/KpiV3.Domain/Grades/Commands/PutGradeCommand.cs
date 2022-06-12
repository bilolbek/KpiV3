using KpiV3.Domain.Grades.DataContracts;
using KpiV3.Domain.Grades.Services;
using MediatR;

namespace KpiV3.Domain.Grades.Commands;

public record PutGradeCommand : IRequest
{
    public Guid EmployeeId { get; init; }
    public Guid RequirementId { get; init; }
    public double Value { get; init; }
}

public class PutGradeCommandHandler : AsyncRequestHandler<PutGradeCommand>
{
    private readonly KpiContext _db;
    private readonly IDateProvider _dateProvider;
    private readonly GradeValidationService _gradeValidationService;

    public PutGradeCommandHandler(
        KpiContext db,
        IDateProvider dateProvider,
        GradeValidationService gradeValidationService)
    {
        _db = db;
        _dateProvider = dateProvider;
        _gradeValidationService = gradeValidationService;
    }

    protected override async Task Handle(PutGradeCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);

        var grade = await _db.Grades.FirstOrDefaultAsync(g =>
            g.EmployeeId == request.EmployeeId &&
            g.RequirementId == request.RequirementId, cancellationToken);

        if (grade is null)
        {
            grade = new Grade
            {
                EmployeeId = request.EmployeeId,
                RequirementId = request.RequirementId,
                Value = request.Value,
                GradedDate = _dateProvider.Now(),
            };

            _db.Grades.Add(grade);
        }
        else
        {
            grade.Value = request.Value;
        }

        await _gradeValidationService.ValidateGradeAsync(grade, cancellationToken);

        await _db.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }
}
