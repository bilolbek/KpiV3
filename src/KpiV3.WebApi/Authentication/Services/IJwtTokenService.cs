using KpiV3.WebApi.Authentication.DataContracts;

namespace KpiV3.WebApi.Authentication.Services;

public interface IJwtTokenService
{
    Task<JwtToken> CreateTokenAsync(
        Credentials credentials, 
        CancellationToken cancellationToken = default);
}
