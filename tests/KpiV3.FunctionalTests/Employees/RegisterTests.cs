using KpiV3.Domain.Employees.Ports;
using KpiV3.WebApi.DataContracts.Employees;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using FluentAssertions;
using System.Net.Http.Json;
using KpiV3.Domain.Employees.DataContracts;

namespace KpiV3.FunctionalTests.Employees;

public class RegisterTests
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
        var application = new WebApplicationFactory<Program>().WithWebHostBuilder(host =>
        {
            host.ConfigureServices((host, services) =>
            {
                services.Replace(ServiceDescriptor.Transient(_ => _employeeRepository.Object));
            });
        });

        return application.CreateClient();
    }
}
