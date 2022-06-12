using KpiV3.Domain.Common.DataContracts;

namespace KpiV3.Domain.Employees.DataContracts;

public record ImportedEmployee
{
    public string Email { get; init; } = default!;
    public Name Name { get; init; } = default!;
    public string Position { get; init; } = default!;
}
