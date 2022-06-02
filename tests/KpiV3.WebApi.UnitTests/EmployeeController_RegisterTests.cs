using FluentAssertions;
using KpiV3.Domain.Employees.Commands;
using KpiV3.WebApi.Controllers;
using KpiV3.WebApi.DataContracts.Employees;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace KpiV3.WebApi.UnitTests;

public class EmployeeController_RegisterTests
{
    private readonly Mock<IMediator> _mediator = new();

    [Fact]
    public async Task Returns_ok_if_mediator_returns_ok()
    {
        // Arrange
        var controller = new EmployeeController(_mediator.Object);

        var request = CreateRequest();

        _mediator
            .Setup(m => m.Send(It.IsAny<RegisterEmployeeCommand>(), default))
            .ReturnsAsync(Result<IError>.Ok());

        // Act
        var response = await controller.Register(request);

        // Assert
        response.Should().BeOfType<OkResult>();
    }

    [Fact]
    public async Task Maps_request_to_command_correctly()
    {
        // Arrange
        var controller = new EmployeeController(_mediator.Object);

        var request = CreateRequest();

        _mediator
            .Setup(m => m.Send(It.Is<RegisterEmployeeCommand>(command =>
                command.Email == request.Email &&
                command.Name.FirstName == request.FirstName &&
                command.Name.LastName == request.LastName &&
                command.Name.MiddleName == request.MiddleName &&
                command.PositionId == request.PositionId), default))
            .ReturnsAsync(Result<IError>.Ok());

        // Act
        await controller.Register(request);

        // Assert
        _mediator.Verify(m => m.Send(It.Is<RegisterEmployeeCommand>(command =>
                command.Email == request.Email &&
                command.Name.FirstName == request.FirstName &&
                command.Name.LastName == request.LastName &&
                command.Name.MiddleName == request.MiddleName &&
                command.PositionId == request.PositionId), default));
    }

    private RegisterEmployeeRequest CreateRequest()
    {
        return new RegisterEmployeeRequest
        {
            Email = "employee@kpi.com",
            FirstName = "Carl",
            MiddleName = "Ashland",
            LastName = "Waterson",
            PositionId = new("253ae920-1b0a-44c6-9c01-ff9b59af77c6")
        };
    }
}
