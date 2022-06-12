using KpiV3.Domain.Grades.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KpiV3.Domain.Grades.Commands;

public record UpdateGradeCommand : IRequest
{
    public Guid EmployeeId { get; init; }
    public Guid RequirementId { get; init; }
    public double Value { get; init; }
}

public class UpdateGradeCommandHandler : AsyncRequestHandler<UpdateGradeCommand>
{
    private readonly KpiContext _db;
    private readonly GradeValidationService _gradeValidationService;

    public UpdateGradeCommandHandler(
        KpiContext db,
        GradeValidationService gradeValidationService)
    {
        _db = db;
        _gradeValidationService = gradeValidationService;
    }

    protected override async Task Handle(UpdateGradeCommand request, CancellationToken cancellationToken)
    {
        var grade = await _db.Grades
            .FirstOrDefaultAsync(g =>
                g.EmployeeId == request.EmployeeId &&
                g.RequirementId == request.RequirementId, cancellationToken)
            .EnsureFoundAsync();

        grade.Value = request.Value;

        await _gradeValidationService.ValidateGradeAsync(grade, cancellationToken);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
