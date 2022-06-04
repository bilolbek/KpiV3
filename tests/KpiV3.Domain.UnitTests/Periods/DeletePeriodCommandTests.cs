using KpiV3.Domain.Periods.Commands;
using KpiV3.Domain.Periods.Ports;
using Moq;

namespace KpiV3.Domain.UnitTests.Periods;

public class DeletePeriodCommandTests
{
    private readonly Mock<IPeriodRepository> _repository = new();

    [Fact]
    public async Task Deletes_period_on_happy_path()
    {
        // Arrange
        var command = new DeletePeriodCommand { PeriodId = Guid.NewGuid() };
        var handler = new DeletePeriodCommandHandler(_repository.Object);

        // Act
        await handler.Handle(command, default);

        // Assert
        _repository.Verify(r => r.DeleteAsync(command.PeriodId));
    }
}
