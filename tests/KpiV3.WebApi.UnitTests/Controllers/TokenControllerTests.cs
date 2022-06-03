using FluentAssertions;
using KpiV3.WebApi.Authentication;
using KpiV3.WebApi.Authentication.DataContracts;
using KpiV3.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace KpiV3.WebApi.UnitTests.Controllers;

public class TokenControllerTests
{
    private readonly Mock<IJwtTokenProvider> _tokenProvider = new();

    [Fact]
    public async Task Returns_ok_object_result_when_token_factory_returns_token()
    {
        // Arrange
        var controller = new TokenController(_tokenProvider.Object);

        var credentials = new Credentials
        {
            Email = "employee@kpi.com",
            Password = "qwerty123"
        };

        var token = new JwtToken
        {
            AccessToken = "TOKEN"
        };

        _tokenProvider
            .Setup(t => t.CreateToken(It.IsAny<Credentials>()))
            .ReturnsAsync(Result<JwtToken, IError>.Ok(token));

        // Act
        var response = await controller.CreateAsync(credentials);

        // Assert
        response.Should().BeOfType<OkObjectResult>();
        var result = response.As<OkObjectResult>();
        result.Value.Should().BeOfType<JwtToken>();
        result.Value.As<JwtToken>().Should().Be(token);
    }

    [Fact]
    public async Task Sends_credentials_to_token_factory()
    {
        // Arrange
        var controller = new TokenController(_tokenProvider.Object);

        var credentials = new Credentials
        {
            Email = "employee@kpi.com",
            Password = "qwerty123"
        };

        var token = new JwtToken
        {
            AccessToken = "TOKEN"
        };

        _tokenProvider
            .Setup(t => t.CreateToken(credentials))
            .ReturnsAsync(Result<JwtToken, IError>.Ok(token));

        // Act
        var response = await controller.CreateAsync(credentials);

        // Assert
        _tokenProvider
            .Verify(t => t.CreateToken(credentials));
    }
}
