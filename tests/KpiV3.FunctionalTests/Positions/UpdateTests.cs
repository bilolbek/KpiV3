using KpiV3.Domain.Positions.DataContracts;
using KpiV3.WebApi.DataContracts.Positions;
using System.Net.Http.Json;
using FluentAssertions;

namespace KpiV3.FunctionalTests.Positions;

public class UpdateTests : TestBase
{
    public UpdateTests()
    {
        Authentication.Authenticate(PositionType.Root);
    }

    [Fact]
    public async Task Returns_ok_and_position_on_happy_path()
    {
        // Arrange
        var request = CreateRequest();

        // Arrange
        var position = new Position
        {
            Id = Guid.NewGuid(),
            Name = "Old name",
            Type = PositionType.Employee,
        };

        Positions.Items[position.Id] = position;

        // Act
        var response = await Client.PutAsJsonAsync($"api/v3.0/position/{position.Id}", request);

        // Assert
        response.Should().Be200Ok().And.Satisfy<PositionDto>(position =>
        {
            position.Id.Should().Be(position.Id);
            position.Name.Should().Be(request.Name);
        });
    }

    [Fact]
    public async Task Returns_not_found_if_position_not_exists()
    {
        // Arrange
        var request = CreateRequest();

        // Act
        var response = await Client.PutAsJsonAsync($"api/v3.0/position/{Guid.NewGuid()}", request);

        // Assert
        response.Should().Be404NotFound();
    }

    private UpdatePositionRequest CreateRequest()
    {
        return new UpdatePositionRequest
        {
            Name = "NewName",
        };
    }
}
