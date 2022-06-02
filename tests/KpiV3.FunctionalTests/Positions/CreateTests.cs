using FluentAssertions;
using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Ports;
using KpiV3.WebApi.DataContracts.Positions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System.Net.Http.Json;

namespace KpiV3.FunctionalTests.Positions;

public class CreateTests
{
    private readonly Mock<IPositionRepository> _positionRepository = new();

    [Fact]
    public async Task Returns_ok_and_created_position_on_happy_path()
    {
        // Arrange
        var client = CreateClient();
        var request = CreateRequest();

        _positionRepository
            .Setup(r => r.InsertAsync(It.IsAny<Position>()))
            .ReturnsAsync(Result<IError>.Ok());

        // Act
        var response = await client.PostAsJsonAsync("api/v3.0/position", request);

        // Assert
        response.Should().Be200Ok().And.Satisfy<CreatePositionResponse>(position =>
        {
            position.Id.Should().NotBeEmpty();
            position.Name.Should().Be(request.Name);
        });
    }

    [Fact]
    public async Task Returns_bad_request_on_invalid_request()
    {
        // Arrange
        var client = CreateClient();
        var request = CreateRequest() with { Name = "" };

        // Act
        var response = await client.PostAsJsonAsync("api/v3.0/position", request);

        // Assert
        response.Should().Be400BadRequest();
    }

    private static CreatePositionRequest CreateRequest()
    {
        return new CreatePositionRequest
        {
            Name = "Teacher"
        };
    }

    private HttpClient CreateClient()
    {
        var application = new WebApplicationFactory<Program>().WithWebHostBuilder(host =>
        {
            host.ConfigureServices((host, services) =>
            {
                services.Replace(ServiceDescriptor.Transient(_ => _positionRepository.Object));
            });
        });

        return application.CreateClient();
    }
}
