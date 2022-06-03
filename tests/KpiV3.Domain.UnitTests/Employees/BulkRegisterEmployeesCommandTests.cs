using FluentAssertions;
using KpiV3.Domain.Employees.Commands;
using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Ports;
using MediatR;
using Moq;

namespace KpiV3.Domain.UnitTests.Employees;

public class BulkRegisterEmployeesCommandTests
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
                Id = new("a6140653-e9a0-464a-99e4-11481eabf017"),
                Name = command.Employees[0].Position
            }));

        _positionRepository
            .Setup(r => r.FindByNameAsync(command.Employees[1].Position))
            .ReturnsAsync(Result<Position, IError>.Ok(new()
            {
                Id = new("291ec7bf-7549-45c1-a597-7cb1d6fd4a2b"),
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

    private BulkRegisterEmployeesCommand CreateCommand()
    {
        return new BulkRegisterEmployeesCommand
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

    private BulkRegisterEmployeesCommandHandler CreateHandler()
    {
        return new BulkRegisterEmployeesCommandHandler(
            _positionRepository.Object,
            _mediator.Object);
    }
}
