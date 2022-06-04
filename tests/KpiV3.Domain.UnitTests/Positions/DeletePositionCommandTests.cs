using FluentAssertions;
using KpiV3.Domain.Positions.Commands;
using KpiV3.Domain.Positions.DataContracts;
using KpiV3.Domain.Positions.Ports;
using Moq;

namespace KpiV3.Domain.UnitTests.Positions;

public class DeletePositionCommandTests
{
    private readonly Mock<IPositionRepository> _repository = new();

    [Fact]
    public async Task Deletes_position_on_happy_path()
    {
        // Arrange
        var command = CreateCommand();
        var handler = CreateHandler();

        _repository
            .Setup(r => r.FindByIdAsync(command.PositionId))
            .ReturnsAsync(Result<Position, IError>.Ok(new Position
            {
                Id = command.PositionId,
                Type = PositionType.Employee
            }));

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _repository.Verify(r => r.DeleteAsync(command.PositionId));
    }

    [Fact]
    public async Task Returns_business_error_if_position_to_delete_is_root()
    {
        // Arrange
        var command = CreateCommand();
        var handler = CreateHandler();

        _repository
            .Setup(r => r.FindByIdAsync(command.PositionId))
            .ReturnsAsync(Result<Position, IError>.Ok(new Position { Type = PositionType.Root }));

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Failure.Should().BeOfType<BusinessRuleViolation>();
    }

    private DeletePositionCommand CreateCommand()
    {
        return new DeletePositionCommand { PositionId = Guid.NewGuid() };
    }

    private DeletePositionCommandHandler CreateHandler()
    {
        return new DeletePositionCommandHandler(_repository.Object);
    }
}
