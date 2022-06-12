namespace KpiV3.WebApi.Authentication.DataContracts;

public readonly record struct JwtToken
{
    public string AccessToken { get; init; }
}
