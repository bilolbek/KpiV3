using FluentAssertions;
using KpiV3.Domain.Common;
using KpiV3.Domain.Positions.Commands;
using KpiV3.Domain.Positions.DataContracts;
using KpiV3.Domain.Positions.Ports;
using Moq;

namespace KpiV3.Domain.UnitTests.Positions;

public class CreatePositionCommandTests
{
    private static readonly Guid PositionId = Guid.NewGuid();

    private readonly Mock<IGuidProvider> _guidProvider = new();
    private readonly Mock<IPositionRepository> _positionRepository = new();

    [Fact]
    public async Task Returns_inserted_position_on_happy_path()
    {
        // Arrange
        var command = CreateCommand();
        var handler = CreateHandler();

        _positionRepository
            .Setup(repository => repository.InsertAsync(It.IsAny<Position>()))
            .ReturnsAsync(Result<IError>.Ok());

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Success.Id.Should().Be(PositionId);
        result.Success.Name.Should().Be(command.Name);
    }

    [Fact]
    public async Task Inserts_position_in_database()
    {
        // Arrange
        var command = CreateCommand();
        var handler = CreateHandler();

        _positionRepository
            .Setup(repository => repository.InsertAsync(It.IsAny<Position>()))
            .ReturnsAsync(Result<IError>.Ok());

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        _positionRepository.Verify(repository => repository.InsertAsync(
            It.Is<Position>(position =>
                position.Id == PositionId &&
                position.Name == command.Name)));
    }

    private CreatePositionCommand CreateCommand()
    {
        return new CreatePositionCommand
        {
            Name = "Teacher"
        };
    }

    private CreatePositionCommandHandler CreateHandler()
    {
        _guidProvider
            .Setup(provider => provider.New())
            .Returns(PositionId);

        return new CreatePositionCommandHandler(
            _guidProvider.Object,
            _positionRepository.Object);
    }
}
