using FluentAssertions;
using KpiV3.Domain.Positions.DataContracts;
using KpiV3.WebApi.DataContracts.Positions;
using System.Net.Http.Json;

namespace KpiV3.FunctionalTests.Positions;

public class CreateTests : TestBase
{
    public CreateTests()
    {
        Authentication.Authenticate(PositionType.Root);
    }

    [Fact]
    public async Task Returns_ok_and_created_position_on_happy_path()
    {
        // Arrange
        var request = CreateRequest();

        // Act
        var response = await Client.PostAsJsonAsync("api/v3.0/position", request);

        // Assert
        response.Should().Be200Ok().And.Satisfy<PositionDto>(position =>
        {
            position.Id.Should().NotBeEmpty();
            position.Name.Should().Be(request.Name);
        });
    }

    [Fact]
    public async Task Creates_position_on_happy_path()
    {
        // Arrange
        var request = CreateRequest();

        // Act
        await Client.PostAsJsonAsync("api/v3.0/position", request);

        // Assert
        Positions.Items.Values.Should().Contain(p => p.Name == request.Name);
    }

    [Fact]
    public async Task Returns_bad_request_on_invalid_request()
    {
        // Arrange
        var request = CreateRequest() with { Name = "" };

        // Act
        var response = await Client.PostAsJsonAsync("api/v3.0/position", request);

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
}
