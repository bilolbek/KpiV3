namespace KpiV3.Domain.Reports.DataContracts;

public class Report
{
    public string Period { get; set; } = default!;
    public DateTimeOffset CreatedDate { get; set; }
    public List<EmployeeReport> EmployeeReports { get; set; } = default!;
}
