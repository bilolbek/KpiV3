namespace KpiV3.Domain.SpecialtyChoices.DataContracts;

public record SpecialtyChoice
{
    public Guid EmployeeId { get; set; }
    public Guid PeriodId { get; set; } 
    public Guid SpecialtyId { get; set; }
    public bool CanBeChanged { get; set; }
}
