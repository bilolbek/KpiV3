using FluentAssertions;
using KpiV3.Domain.Employees.Commands;
using KpiV3.Domain.Positions.DataContracts;
using KpiV3.Domain.Positions.Ports;
using MediatR;
using Moq;

namespace KpiV3.Domain.UnitTests.Employees;

public class ImportEmployeesCommandTests
{
    private readonly Mock<IPositionRepository> _positionRepository = new();
    private readonly Mock<IMediator> _mediator = new();

    [Fact]
    public async Task Registers_every_employee_on_happy_path()
    {
        // Arrange
        var command = CreateCommand();
        var handler = CreateHandler();

        _positionRepository
            .Setup(r => r.FindByNameAsync(command.Employees[0].Position))
            .ReturnsAsync(Result<Position, IError>.Ok(new()
            {
                Id = Guid.NewGuid(),
                Name = command.Employees[0].Position
            }));

        _positionRepository
            .Setup(r => r.FindByNameAsync(command.Employees[1].Position))
            .ReturnsAsync(Result<Position, IError>.Ok(new()
            {
                Id = Guid.NewGuid(),
                Name = command.Employees[1].Position
            }));

        _mediator
            .Setup(m => m.Send(It.IsAny<RegisterEmployeeCommand>(), default))
            .ReturnsAsync(Result<IError>.Ok());

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _mediator.Verify(m => m.Send(
            It.Is<RegisterEmployeeCommand>(c =>
                c.Name.FirstName == command.Employees[0].FirstName &&
                c.Name.LastName == command.Employees[0].LastName &&
                c.Name.MiddleName == command.Employees[0].MiddleName &&
                c.Email == command.Employees[0].Email),
            default));

        _mediator.Verify(m => m.Send(
            It.Is<RegisterEmployeeCommand>(c =>
                c.Name.FirstName == command.Employees[1].FirstName &&
                c.Name.LastName == command.Employees[1].LastName &&
                c.Name.MiddleName == command.Employees[1].MiddleName &&
                c.Email == command.Employees[1].Email),
            default));
    }

    [Fact]
    public async Task Propagates_error_returned_by_repository()
    {
        // Arrange
        var command = CreateCommand();
        var handler = CreateHandler();

        var expectedError = new TestError("TestError");

        _positionRepository
            .Setup(r => r.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(Result<Position, IError>.Fail(expectedError));

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.Failure.Should().Be(expectedError);
    }

    private ImportEmployeesCommand CreateCommand()
    {
        return new ImportEmployeesCommand
        {
            Employees = new()
            {
                new()
                {
                    Email = "employee1@kpi.com",

                    FirstName = "Employee1",
                    LastName = "Employee1",
                    MiddleName = "Employee1",

                    Position = "Teacher"
                },

                new()
                {
                    Email = "employee2@kpi.com",

                    FirstName = "Employee2",
                    LastName = "Employee2",
                    MiddleName = null,

                    Position = "Dean"
                },
            },
        };
    }

    private ImportEmployeesCommandHandler CreateHandler()
    {
        return new ImportEmployeesCommandHandler(
            _positionRepository.Object,
            _mediator.Object);
    }
}
