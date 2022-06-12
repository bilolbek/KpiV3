using KpiV3.Domain;
using KpiV3.Domain.Employees.Ports;
using KpiV3.WebApi.Authentication.DataContracts;
using Microsoft.EntityFrameworkCore;

namespace KpiV3.WebApi.Authentication.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly IJwtTokenFactory _tokenFactory;
    private readonly KpiContext _db;
    private readonly IPasswordHasher _passwordHasher;

    public JwtTokenService(
        IJwtTokenFactory tokenFactory, 
        KpiContext db, 
        IPasswordHasher passwordHasher)
    {
        _tokenFactory = tokenFactory;
        _db = db;
        _passwordHasher = passwordHasher;
    }

    public async Task<JwtToken> CreateTokenAsync(Credentials credentials, CancellationToken cancellationToken = default)
    {
        var employee = await _db.Employees
            .Include(e => e.Position)
            .FirstOrDefaultAsync(e => e.Email == credentials.Email, cancellationToken);

        if (employee is null || !_passwordHasher.Verify(credentials.Password, employee.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        return _tokenFactory.CreateToken(employee);
    }
}
