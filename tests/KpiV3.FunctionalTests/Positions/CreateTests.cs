using FluentAssertions;
using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Ports;
using KpiV3.WebApi.DataContracts.Positions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System.Net.Http.Json;

namespace KpiV3.FunctionalTests.Positions;

public class CreateTests : TestBase
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
        Requestor = new Employee
        {
            Id = new("075bcdf4-0457-4d7b-a9d5-a2b8d3ef7deb")
        };

        RequestorPosition = new Position
        {
            Id = new("96c06b84-f752-4cf3-b729-ce5399af6434"),
            Name = "Admin",
            Type = PositionType.Root,
        };

        return Authorize(CreateClient((env, services) =>
        {
            services.Replace(ServiceDescriptor.Transient(_ => _positionRepository.Object));
        }));
    }
}
