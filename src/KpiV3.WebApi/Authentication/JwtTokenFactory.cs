using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Ports;
using KpiV3.Domain.Positions.DataContracts;
using KpiV3.WebApi.Authentication.DataContracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace KpiV3.WebApi.Authentication;

public class JwtTokenFactory : IJwtTokenFactory
{
    private readonly IDateProvider _dateProvider;
    private readonly JwtOptions _options;

    public JwtTokenFactory(
        IDateProvider dateProvider,
        IOptions<JwtOptions> options)
    {
        _dateProvider = dateProvider;
        _options = options.Value;
    }

    public JwtToken CreateToken(Employee employee, Position position)
    {
        var handler = new JsonWebTokenHandler();

        var now = _dateProvider.Now();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            IssuedAt = now.UtcDateTime,
            Expires = now.Add(_options.TokenLifetime).UtcDateTime,
            Claims = new Dictionary<string, object>
            {
                { "sub", employee.Id.ToString() },
                { "posId", position.Id.ToString() },
                { "posName", position.Name },
                { "posType", position.Type.ToString() },
                { "firstName", employee.Name.FirstName },
                { "lastName", employee.Name.LastName },
            },
            SigningCredentials = new SigningCredentials(
                _options.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256),
        };

        if (employee.Name.MiddleName is not null)
        {
            tokenDescriptor.Claims["middleName"] = employee.Name.MiddleName;
        }

        return new JwtToken
        {
            AccessToken = handler.CreateToken(tokenDescriptor)
        };
    }
}
