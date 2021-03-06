using KpiV3.Domain.Common.DataContracts;

namespace KpiV3.Domain.Tasklists.DataContracts;

public class AdminTasklistRow
{
    public Profile Employee { get; set; } = default!;
    public Guid? SpecialtyId { get; set; }

    public double TotalGrade { get; set; }
    public double TotalWeight { get; set; }
    public int SubmissionsCount { get; set; }
    public int RequirementsCount { get; set; }
}
