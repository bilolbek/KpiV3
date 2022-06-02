namespace KpiV3.Domain.DataContracts.Models;

public readonly record struct Name
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string? MiddleName { get; init; }
}
