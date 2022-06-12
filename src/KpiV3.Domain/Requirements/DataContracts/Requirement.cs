using KpiV3.Domain.Grades.DataContracts;
using KpiV3.Domain.Indicators.DataContracts;
using KpiV3.Domain.PeriodParts.DataContracts;
using KpiV3.Domain.Specialties.DataContracts;
using KpiV3.Domain.Submissions.DataContracts;

namespace KpiV3.Domain.Requirements.DataContracts;

public class Requirement
{
    public Guid Id { get; set; }

    public string? Note { get; set; }

    public double Weight { get; set; }

    public Guid PeriodPartId { get; set; }
    public PeriodPart PeriodPart { get; set; } = default!;

    public Guid SpecialtyId { get; set; }
    public Specialty Specialty { get; set; } = default!;

    public Guid IndicatorId { get; set; }
    public Indicator Indicator { get; set; } = default!;

    public ICollection<Submission> Submissions { get; set; } = default!;
    public ICollection<Grade> Grades { get; set; } = default!;
}
