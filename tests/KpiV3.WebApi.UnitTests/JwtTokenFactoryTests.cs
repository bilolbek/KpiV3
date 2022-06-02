using FluentAssertions;
using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Ports;
using KpiV3.Domain.Ports;
using KpiV3.WebApi.Authentication;
using KpiV3.WebApi.Authentication.DataContracts;
using Microsoft.Extensions.Options;
using Moq;

namespace KpiV3.WebApi.UnitTests;

public class JwtTokenFactoryTests
{
    private static readonly Position Position = new()
    {
        Id = new("ff943168-f376-41ff-b0c1-77648ecffbe4"),
        Name = "Dean",
    };

    private static readonly Employee Employee = new()
    {
        Id = new("40502ac9-92e4-4bcb-b382-ea31103efec8"),
        Email = "employee@kpi.com",
        Name = new()
        {
            FirstName = "Alex",
            LastName = "Ithamar",
            MiddleName = "Martel",
        },
        PasswordHash = "some_random_hash",
        PositionId = Position.Id,
        RegistrationDate = new DateTimeOffset(2022, 10, 5, 9, 15, 0, TimeSpan.Zero),
    };

    private static readonly Credentials Credentials = new()
    {
        Email = "employee@kpi.com",
        Password = "qwerty123",
    };

    private readonly Mock<IEmployeeRepository> _employeeRepository = new();
    private readonly Mock<IPositionRepository> _positionRepository = new();
    private readonly Mock<IPasswordHasher> _passwordhasher = new();
    private readonly Mock<IDateProvider> _dateProvider = new();

    [Fact]
    public async Task Should_return_token_when_employee_exists()
    {
        // Arrange
        var factory = new JwtTokenFactory(
            _employeeRepository.Object,
            _positionRepository.Object,
            _passwordhasher.Object,
            _dateProvider.Object,
            Options.Create(CreateOptions()));

        _employeeRepository
            .Setup(r => r.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(Result<Employee, IError>.Ok(Employee));

        _positionRepository
            .Setup(r => r.FindByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(Result<Position, IError>.Ok(Position));

        _passwordhasher
            .Setup(h => h.Verify(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        // Act
        var result = await factory.CreateToken(Credentials);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Success.AccessToken.Should().NotBeNullOrEmpty();
    }

    private JwtOptions CreateOptions()
    {
        return new JwtOptions
        {
            Issuer = "test_issuer",
            Audience = "test_audience",
            Secret = "test_secret_set_only_for_testing_purposes",
            TokenLifetime = TimeSpan.FromHours(1),
        };
    }
}
