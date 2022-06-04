using FluentAssertions;
using KpiV3.Domain.Positions.Commands;
using KpiV3.Domain.Positions.DataContracts;
using KpiV3.Domain.Positions.Ports;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KpiV3.Domain.UnitTests.Positions;

public class UpdatePositionCommandTests
{
    private readonly Mock<IPositionRepository> _repository = new();

    [Fact]
    public async Task Updates_position_on_happy_path()
    {
        // Arrange
        var command = CreateCommand();
        var handler = CreateHandler();

        var position = new Position
        {
            Id = command.PositionId,

            Name = "Old name",

            Type = PositionType.Employee,
        };

        _repository
            .Setup(r => r.FindByIdAsync(position.Id))
            .ReturnsAsync(Result<Position, IError>.Ok(position));

        _repository
            .Setup(r => r.UpdateAsync(It.IsAny<Position>()))
            .ReturnsAsync(Result<IError>.Ok());

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        _repository.Verify(r => r.UpdateAsync(
            It.Is<Position>(p =>
                p.Id == position.Id &&
                p.Name == command.Name &&
                p.Type == position.Type)));
    }

    [Fact]
    public async Task Returns_updated_position_on_happy_path()
    {
        // Arrange
        var command = CreateCommand();
        var handler = CreateHandler();

        var position = new Position
        {
            Id = command.PositionId,

            Name = "Old name",

            Type = PositionType.Employee,
        };

        _repository
            .Setup(r => r.FindByIdAsync(position.Id))
            .ReturnsAsync(Result<Position, IError>.Ok(position));

        _repository
            .Setup(r => r.UpdateAsync(It.IsAny<Position>()))
            .ReturnsAsync(Result<IError>.Ok());

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Success.Id.Should().Be(position.Id);
        result.Success.Name.Should().Be(command.Name);
        result.Success.Type.Should().Be(position.Type);
    }

    [Fact]
    public async Task Returns_business_error_if_position_type_is_root()
    {
        // Arrange
        var command = CreateCommand();
        var handler = CreateHandler();

        var position = new Position
        {
            Id = command.PositionId,

            Name = "Old name",

            Type = PositionType.Root,
        };

        _repository
            .Setup(r => r.FindByIdAsync(position.Id))
            .ReturnsAsync(Result<Position, IError>.Ok(position));

        // Act
        var result = await handler.Handle(command, default);

        // Arrange
        result.IsFailure.Should().BeTrue();
        result.Failure.Should().BeOfType<BusinessRuleViolation>();
    }

    private UpdatePositionCommand CreateCommand()
    {
        return new UpdatePositionCommand
        {
            PositionId = Guid.NewGuid(),
            Name = "New name",
        };
    }

    private UpdatePositionCommandHandler CreateHandler()
    {
        return new UpdatePositionCommandHandler(_repository.Object);
    }
}
