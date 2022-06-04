using FluentAssertions;
using KpiV3.Domain.Positions.DataContracts;

namespace KpiV3.FunctionalTests.Positions;

public class DeleteTests : TestBase
{
    public DeleteTests()
    {
        Authentication.Authenticate(PositionType.Root);
    }

    [Fact]
    public async Task Returns_ok_on_happy_path()
    {
        // Arrange
        var position = new Position
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Type = PositionType.Employee,
        };

        Positions.Items[position.Id] = position;

        // Act
        var response = await Client.DeleteAsync($"api/v3.0/position/{position.Id}");

        // Assert
        response.Should().Be200Ok();
    }

    [Fact]
    public async Task Returns_not_found_if_position_is_not_found()
    {
        // Arrange        

        // Act
        var response = await Client.DeleteAsync($"api/v3.0/position/{Guid.NewGuid()}");

        // Assert
        response.Should().Be404NotFound();
    }
}
