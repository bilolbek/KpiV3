using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Periods.DataContracts;
using KpiV3.Domain.Specialties.DataContracts;

namespace KpiV3.Domain.SpecialtyChoices.DataContracts;

public record SpecialtyChoice
{
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = default!;

    public Guid SpecialtyId { get; set; }
    public Specialty Specialty { get; set; } = default!;

    public Guid PeriodId { get; set; }
    public Period Period { get; set; } = default!;

    public bool CanBeChanged { get; set; }
}
