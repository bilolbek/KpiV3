using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Ports;
using KpiV3.Domain.Ports;
using KpiV3.WebApi.Authentication.DataContracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace KpiV3.WebApi.Authentication;

public class JwtTokenFactory : IJwtTokenFactory
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IPositionRepository _positionRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IDateProvider _dateProvider;
    private readonly JwtOptions _options;

    public JwtTokenFactory(
        IEmployeeRepository employeeRepository,
        IPositionRepository positionRepository,
        IPasswordHasher passwordHasher,
        IDateProvider dateProvider,
        IOptions<JwtOptions> options)
    {
        _employeeRepository = employeeRepository;
        _positionRepository = positionRepository;
        _passwordHasher = passwordHasher;
        _dateProvider = dateProvider;
        _options = options.Value;
    }

    public async Task<Result<JwtToken, IError>> CreateToken(Credentials credentials)
    {
        return await _employeeRepository
            .FindByEmailAsync(credentials.Email)
            .MapFailureAsync(error => error is NoEntity ? InvalidCredentials() : error)
            .BindAsync(employee => VerifyPassword(employee, credentials.Password))
            .BindAsync(employee => _positionRepository
                .FindByIdAsync(employee.PositionId)
                .MapAsync(position => CreateToken(employee, position)));
    }

    private JwtToken CreateToken(Employee employee, Position position)
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

    private Result<Employee, IError> VerifyPassword(Employee employee, string password)
    {
        if (_passwordHasher.Verify(password, employee.PasswordHash))
        {
            return Result<Employee, IError>.Ok(employee);
        }

        return Result<Employee, IError>.Fail(InvalidCredentials());
    }

    private static UnauthorizedAccess InvalidCredentials() => new("Invalid credentials");
}
