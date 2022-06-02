using FluentAssertions;
using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Ports;
using KpiV3.WebApi.Authentication.DataContracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System.Net.Http.Json;

namespace KpiV3.FunctionalTests;

public class CreateTokenTests : TestBase
{
    private readonly Mock<IPositionRepository> _positionRepository = new();
    private readonly Mock<IEmployeeRepository> _employeeRepository = new();
    private readonly Mock<IPasswordHasher> _passwordHasher = new();

    [Fact]
    public async Task Returns_ok_with_jwt_token_on_happy_path()
    {
        // Arrange
        var client = CreateClient();

        var credentials = CreateCredentials();

        _employeeRepository
            .Setup(r => r.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(Result<Employee, IError>.Ok(new Employee { }));

        _positionRepository
            .Setup(r => r.FindByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(Result<Position, IError>.Ok(new Position { }));

        _passwordHasher
            .Setup(h => h.Verify(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        // Act
        var response = await client.PostAsJsonAsync("api/v3.0/token", credentials);

        // Assert
        response.Should().Be200Ok().And.Satisfy<JwtToken>(jwt =>
        {
            jwt.AccessToken.Should().NotBeNullOrEmpty();
        });
    }

    [Fact]
    public async Task Returns_unauthorized_when_repository_returns_no_entity()
    {
        // Arrange
        var client = CreateClient();

        var credentials = CreateCredentials();

        _employeeRepository
            .Setup(r => r.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(Result<Employee, IError>.Fail(new NoEntity(typeof(Employee))));

        // Act
        var response = await client.PostAsJsonAsync("api/v3.0/token", credentials);

        // Assert
        response.Should().Be401Unauthorized();
    }

    [Theory]
    [MemberData(nameof(InvalidCredentials))]
    public async Task Returns_bad_request_on_invalid_credentials(Credentials credentials)
    {
        // Arrange
        var client = CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("api/v3.0/token", credentials);

        // Assert
        response.Should().Be400BadRequest();
    }

    [Fact]
    public async Task Returns_unauthorized_when_password_verify_failed()
    {
        // Arrange
        var client = CreateClient();

        var credentials = CreateCredentials();

        _employeeRepository
            .Setup(r => r.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(Result<Employee, IError>.Ok(new Employee { }));

        _positionRepository
            .Setup(r => r.FindByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(Result<Position, IError>.Ok(new Position { }));

        _passwordHasher
            .Setup(h => h.Verify(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(false);

        // Act
        var response = await client.PostAsJsonAsync("api/v3.0/token", credentials);

        // Assert
        response.Should().Be401Unauthorized();
    }

    private static Credentials CreateCredentials()
    {
        return new Credentials
        {
            Email = "employee@kpi.com",
            Password = "qwerty123",
        };
    }

    private static IEnumerable<object[]> InvalidCredentials()
    {
        yield return new object[] { CreateCredentials() with { Email = "" } };
        yield return new object[] { CreateCredentials() with { Password = "" } };
    }

    private HttpClient CreateClient()
    {
        return CreateClient((env, services) =>
        {
            services.Replace(ServiceDescriptor.Transient(_ => _positionRepository.Object));
            services.Replace(ServiceDescriptor.Transient(_ => _employeeRepository.Object));
            services.Replace(ServiceDescriptor.Transient(_ => _passwordHasher.Object));
        });
    }
}
