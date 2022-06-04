using FluentAssertions;
using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Ports;
using KpiV3.Domain.Positions.DataContracts;
using KpiV3.WebApi.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Moq;

namespace KpiV3.WebApi.UnitTests.Authentication;

public class JwtTokenFactoryTests
{
    private readonly Mock<IDateProvider> _dateProvider = new();

    [Fact]
    public void Returns_access_token_with_correct_claims()
    {
        // Arrange
        var options = new JwtOptions
        {
            Audience = "TEST_AUDIENCE",
            Issuer = "TEST_ISSUER",
            Secret = "TEST_SECRET________ONLY_______FOR____TEST",
            TokenLifetime = TimeSpan.FromHours(1)
        };

        var position = new Position
        {
            Id = new("6eb5a797-6b8e-42ca-b528-c9afef35bac7"),
            Name = "Admin",
            Type = PositionType.Root,
        };

        var employee = new Employee
        {
            Id = new("e06fd3b9-10b4-4eca-bec9-1bfa3cb8f3fe"),
            Email = "admin@kpi.com",
            Name = new()
            {
                FirstName = "Admin",
                LastName = "Adminov",
                MiddleName = "Adminovich"
            },
        };

        var now = new DateTimeOffset(2022, 3, 6, 5, 10, 0, TimeSpan.Zero);

        _dateProvider
            .Setup(provider => provider.Now())
            .Returns(now);

        var jwtFactory = new JwtTokenFactory(
            _dateProvider.Object,
            Options.Create(options));

        // Act
        var jwtToken = jwtFactory.CreateToken(employee, position);

        // Assert
        var handler = new JsonWebTokenHandler();

        var token = handler.ReadJsonWebToken(jwtToken.AccessToken);

        token.Claims.Should().Contain(claim => claim.Type == "aud" && claim.Value == options.Audience);
        token.Claims.Should().Contain(claim => claim.Type == "iss" && claim.Value == options.Issuer);
        token.Claims.Should().Contain(claim => claim.Type == "exp" && claim.Value == now.Add(options.TokenLifetime).ToUnixTimeSeconds().ToString());
        token.Claims.Should().Contain(claim => claim.Type == "iat" && claim.Value == now.ToUnixTimeSeconds().ToString());
        token.Claims.Should().Contain(claim => claim.Type == "sub" && claim.Value == employee.Id.ToString());
        token.Claims.Should().Contain(claim => claim.Type == "posId" && claim.Value == position.Id.ToString());
        token.Claims.Should().Contain(claim => claim.Type == "posType" && claim.Value == position.Type.ToString());
        token.Claims.Should().Contain(claim => claim.Type == "firstName" && claim.Value == employee.Name.FirstName);
        token.Claims.Should().Contain(claim => claim.Type == "lastName" && claim.Value == employee.Name.LastName);
        token.Claims.Should().Contain(claim => claim.Type == "middleName" && claim.Value == employee.Name.MiddleName);
    }
}