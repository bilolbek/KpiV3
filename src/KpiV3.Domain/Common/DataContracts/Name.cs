namespace KpiV3.Domain.Common.DataContracts;

public record Name
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? MiddleName { get; set; }
}
