using FluentAssertions;
using KpiV3.Domain.Periods.Commands;
using KpiV3.Domain.Periods.DataContracts;
using KpiV3.Domain.Periods.Ports;
using KpiV3.Domain.Ports;
using Moq;

namespace KpiV3.Domain.UnitTests.Periods;

public class CreatePeriodCommandTests
{
    private readonly Mock<IPeriodRepository> _repository = new();
    private readonly Mock<IGuidProvider> _guidProvider = new();

    private readonly Guid PeriodId = Guid.NewGuid();

    [Fact]
    public async Task Returns_inserted_period_on_happy_path()
    {
        // Arrange
        var command = CreateCommand();
        var handler = CreateHandler();

        _guidProvider
            .Setup(p => p.New())
            .Returns(PeriodId);

        _repository
            .Setup(r => r.InsertAsync(It.IsAny<Period>()))
            .ReturnsAsync(Result<IError>.Ok());

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Success.Id.Should().Be(PeriodId);
        result.Success.Name.Should().Be(command.Name);
        result.Success.From.Should().Be(command.From);
        result.Success.To.Should().Be(command.To);
    }

    [Fact]
    public async Task Inserts_period_into_database()
    {
        // Arrange
        var command = CreateCommand();
        var handler = CreateHandler();

        _guidProvider
            .Setup(p => p.New())
            .Returns(PeriodId);

        _repository
            .Setup(r => r.InsertAsync(It.IsAny<Period>()))
            .ReturnsAsync(Result<IError>.Ok());

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        _repository.Verify(r => r.InsertAsync(
            It.Is<Period>(p =>
                p.Id == PeriodId &&
                p.Name == command.Name &&
                p.From == command.From &&
                p.To == command.To)));
    }

    [Fact]
    public async Task Returns_invalid_input_error_if_from_is_greater_than_to()
    {
        // Arrange
        var command = CreateCommand();
        command = command with { From = command.To.AddTicks(1) };
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Failure.Should().BeOfType<InvalidInput>();
    }

    private CreatePeriodCommand CreateCommand()
    {
        return new CreatePeriodCommand
        {
            Name = "2022 year",
            From = new DateTime(2022, 1, 1),
            To = new DateTime(2022, 12, 31),
        };
    }

    private CreatePeriodCommandHandler CreateHandler()
    {
        return new CreatePeriodCommandHandler(
            _repository.Object,
            _guidProvider.Object);
    }
}
