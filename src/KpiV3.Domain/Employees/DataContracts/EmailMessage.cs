namespace KpiV3.Domain.Employees.DataContracts;

public record EmailMessage
{
    public string Recipient { get; init; } = default!;
    public string Subject { get; init; } = default!;
    public string Body { get; init; } = default!;
}
