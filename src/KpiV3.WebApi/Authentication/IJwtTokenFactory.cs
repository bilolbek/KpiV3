using KpiV3.WebApi.Authentication.DataContracts;

namespace KpiV3.WebApi.Authentication;

public interface IJwtTokenFactory
{
    Task<Result<JwtToken, IError>> CreateToken(Credentials credentials);
}
