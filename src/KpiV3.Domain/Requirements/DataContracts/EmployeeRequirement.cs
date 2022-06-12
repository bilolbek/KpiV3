using KpiV3.Domain.Common.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KpiV3.Domain.Requirements.DataContracts;

public class EmployeeRequirement
{
    public Guid EmployeeId { get; init; }
    public Name EmployeeName { get; init; } = default!;

    public Guid RequirementId { get; init; } = default!;

    public Guid IndicatorId { get; init; } = default!;
    public string IndicatorName { get; init; } = default!;

    public Guid PeriodPartId { get; init; }
    public string PeriodPartName { get; init; } = default!;
}
