using FluentAssertions;
using KpiV3.Domain.Employees.Commands;
using KpiV3.Domain.Employees.DataContracts;
using KpiV3.WebApi.Controllers;
using KpiV3.WebApi.DataContracts.Positions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace KpiV3.WebApi.UnitTests;

public class PositionController_CreateTests
{
    private readonly Mock<IMediator> _mediator = new();

    [Fact]
    public async Task Returns_ok_object_result_with_returned_position()
    {
        // Arrange
        var controller = new PositionController(_mediator.Object);

        var request = CreateRequest();

        var position = new Position
        {
            Id = new("7f859c2c-1971-49bf-b8e3-f56b24d279a7"),
            Name = request.Name,
        };

        _mediator
            .Setup(m => m.Send(It.IsAny<CreatePositionCommand>(), default))
            .ReturnsAsync(Result<Position, IError>.Ok(position));

        // Act
        var response = await controller.CreateAsync(request);

        // Assert
        response.Should().BeOfType<OkObjectResult>();
        var result = response.As<OkObjectResult>();
        result.Value.Should().BeOfType<CreatePositionResponse>();
        result.Value.As<CreatePositionResponse>().Should().Be(new CreatePositionResponse
        {
            Id = position.Id,
            Name = position.Name,
        });
    }

    [Fact]
    public async Task Maps_request_to_command_correctly()
    {
        // Arrange
        var controller = new PositionController(_mediator.Object);

        var request = CreateRequest();

        var position = new Position
        {
            Id = new("7f859c2c-1971-49bf-b8e3-f56b24d279a7"),
            Name = request.Name,
        };

        _mediator
            .Setup(m => m.Send(It.IsAny<CreatePositionCommand>(), default))
            .ReturnsAsync(Result<Position, IError>.Ok(position));
        // Act
        await controller.CreateAsync(request);

        // Assert
        _mediator.Verify(m => m.Send(
            It.Is<CreatePositionCommand>(command => 
                command.Name == request.Name), 
            default));
    }

    private CreatePositionRequest CreateRequest()
    {
        return new CreatePositionRequest
        {
            Name = "Teacher"
        };
    }
}
