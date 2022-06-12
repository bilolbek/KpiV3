using KpiV3.Domain.Grades.DataContracts;

namespace KpiV3.Domain.Grades.Services;

public class GradeValidationService
{
    private readonly KpiContext _db;

    public GradeValidationService(KpiContext db)
    {
        _db = db;
    }

    public async Task ValidateGradeAsync(
        Grade grade,
        CancellationToken cancellationToken = default)
    {
        var requirement = await _db.Requirements
            .FindAsync(new object?[] { grade.Value }, cancellationToken)
            .EnsureFoundAsync();

        if (grade.Value > requirement.Weight)
        {
            throw new BusinessLogicException("Grade value cannot exceed requirement value");
        }
    }
}
