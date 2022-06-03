using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Ports;
using KpiV3.WebApi.Authentication.DataContracts;

namespace KpiV3.WebApi.Authentication;

public class JwtTokenProvider : IJwtTokenProvider
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IPositionRepository _positionRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenFactory _tokenFactory;

    public JwtTokenProvider(
        IEmployeeRepository employeeRepository,
        IPositionRepository positionRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenFactory tokenFactory)
    {
        _employeeRepository = employeeRepository;
        _positionRepository = positionRepository;
        _passwordHasher = passwordHasher;
        _tokenFactory = tokenFactory;
    }

    public async Task<Result<JwtToken, IError>> CreateToken(Credentials credentials)
{
        return await _employeeRepository
            .FindByEmailAsync(credentials.Email)
            .MapFailureAsync(error => error is NoEntity ? InvalidCredentials() : error)
            .BindAsync(employee => VerifyPassword(employee, credentials.Password))
            .BindAsync(employee => _positionRepository
                .FindByIdAsync(employee.PositionId)
                .MapAsync(position => _tokenFactory.CreateToken(employee, position)));
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
