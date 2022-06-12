namespace KpiV3.Domain.Reports.DataContracts;

public record EmployeeReport
{
    public string FullName { get; set; } = default!;

    public string Position { get; set; } = default!;
    public string Specialty { get; set; } = default!;

    public List<ReportItem> Items { get; set; } = default!;

    public double Kpi
    {
        get
        {
            return Items.Select(i => i.Value ?? 0.0).Sum() / Items.Sum(i => i.Weight);
        }
    }
}

public class ReportItem
{
    public string Indicator { get; set; } = default!;
    public double Weight { get; set; } = default!;
    public double? Value { get; set; } = default!;
}