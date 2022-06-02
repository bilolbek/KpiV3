using KpiV3.Domain.Employees.Ports;
using KpiV3.WebApi.DataContracts.Employees;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using FluentAssertions;
using System.Net.Http.Json;
using KpiV3.Domain.Employees.DataContracts;

namespace KpiV3.FunctionalTests.Employees;

public class RegisterTests : TestBase
{
    private readonly Mock<IEmployeeRepository> _employeeRepository = new();

    [Fact]
    public async Task Returns_ok_on_happy_path()
    {
        // Arrange
        var client = CreateClient();
        var request = CreateRequest();

        _employeeRepository
            .Setup(r => r.InsertAsync(It.IsAny<Employee>()))
            .ReturnsAsync(Result<IError>.Ok());

        // Act
        var response = await client.PostAsJsonAsync("api/v3.0/employee", request);

        // Assert
        response.Should().Be200Ok();
    }

    [Theory]
    [MemberData(nameof(InvalidRequests))]
    public async Task Returns_bad_request_if_request_is_invalid(RegisterEmployeeRequest request)
    {
        // Arrange
        var client = CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("api/v3.0/employee", request);

        // Assert
        response.Should().Be400BadRequest();
    }

    private static IEnumerable<object[]> InvalidRequests()
    {
        yield return new object[] { CreateRequest() with { Email = "" } };
        yield return new object[] { CreateRequest() with { FirstName = "" } };
        yield return new object[] { CreateRequest() with { LastName = "" } };
    }

    private static RegisterEmployeeRequest CreateRequest()
    {
        return new RegisterEmployeeRequest
        {
            Email = "employee@kpi.com",
            FirstName = "Carl",
            MiddleName = "Ashland",
            LastName = "Waterson",
            PositionId = new("3834d737-3ebe-4035-b6d8-980f6e76d481"),
        };
    }

    private HttpClient CreateClient()
    {
        Requestor = new Employee
        {
            Id = new("075bcdf4-0457-4d7b-a9d5-a2b8d3ef7deb")
        };

        RequestorPosition = new Position
        {
            Id = new("96c06b84-f752-4cf3-b729-ce5399af6434"),
            Name = "Admin"
        };

        return Authorize(CreateClient((env, services) =>
        {
            services.Replace(ServiceDescriptor.Transient(_ => _employeeRepository.Object));
        }));
    }
}
