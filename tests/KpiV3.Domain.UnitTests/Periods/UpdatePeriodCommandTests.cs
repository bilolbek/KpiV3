using FluentAssertions;
using KpiV3.Domain.Periods.Commands;
using KpiV3.Domain.Periods.DataContracts;
using KpiV3.Domain.Periods.Ports;
using Moq;

namespace KpiV3.Domain.UnitTests.Periods;

public class UpdatePeriodCommandTests
{
    private readonly Mock<IPeriodRepository> _repository = new();

    [Fact]
    public async Task Returns_updated_period_on_happy_path()
    {
        // Arrange
        var command = CreateCommand();
        var handler = CreateHandler();

        _repository
            .Setup(r => r.InsertAsync(It.IsAny<Period>()))
            .ReturnsAsync(Result<IError>.Ok());

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Success.Id.Should().Be(command.PeriodId);
        result.Success.Name.Should().Be(command.Name);
        result.Success.From.Should().Be(command.From);
        result.Success.To.Should().Be(command.To);
    }

    [Fact]
    public async Task Updates_period_in_database_on_happy_path()
    {
        // Arrange
        var command = CreateCommand();
        var handler = CreateHandler();

        _repository
            .Setup(r => r.UpdateAsync(It.IsAny<Period>()))
            .ReturnsAsync(Result<IError>.Ok());

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        _repository.Verify(r => r.UpdateAsync(
            It.Is<Period>(p =>
                p.Id == command.PeriodId &&
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

    private UpdatePeriodCommand CreateCommand()
    {
        return new UpdatePeriodCommand
        {
            PeriodId = Guid.NewGuid(),
            Name = "2022 year",
            From = new DateTime(2022, 1, 1),
            To = new DateTime(2022, 12, 31),
        };
    }

    private UpdatePeriodCommandHandler CreateHandler()
    {
        return new UpdatePeriodCommandHandler(_repository.Object);
    }
}
