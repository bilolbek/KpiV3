using KpiV3.Domain.Common.Ports;
using KpiV3.Domain.Employees.DataContracts;
using KpiV3.WebApi.Authentication.DataContracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace KpiV3.WebApi.Authentication.Services;

public class JwtTokenFactory : IJwtTokenFactory
{
    private readonly JsonWebTokenHandler _handler = new();

    private readonly JwtOptions _options;
    private readonly IDateProvider _dateProvider;

    public JwtTokenFactory(
        IOptions<JwtOptions> options,
        IDateProvider dateProvider)
    {
        _options = options.Value;
        _dateProvider = dateProvider;
    }

    public JwtToken CreateToken(Employee employee)
    {
        var now = _dateProvider.Now().UtcDateTime;

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            IssuedAt = now,
            Expires = now.Add(_options.TokenLifetime),
            Claims = new Dictionary<string, object>
            {
                ["sub"] = employee.Id.ToString(),
                ["posId"] = employee.Position.Id.ToString(),
                ["posType"] = employee.Position.Type.ToString(),
                ["posName"] = employee.Position.Name,
                ["firstName"] = employee.Name.FirstName,
                ["lastName"] = employee.Name.LastName,
                ["middleName"] = employee.Name.MiddleName ?? "",
            },
            SigningCredentials = new SigningCredentials(
                _options.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256),
        };

        return new JwtToken
        {
            AccessToken = _handler.CreateToken(tokenDescriptor),
        };
    }
}
