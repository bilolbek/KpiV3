namespace KpiV3.WebApi.Authentication.DataContracts;

public record Credentials
{
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
}
