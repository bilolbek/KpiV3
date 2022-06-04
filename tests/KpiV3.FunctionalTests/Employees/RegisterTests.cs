using KpiV3.WebApi.DataContracts.Employees;
using FluentAssertions;
using System.Net.Http.Json;
using KpiV3.Domain.Positions.DataContracts;

namespace KpiV3.FunctionalTests.Employees;

public class RegisterTests : TestBase
{
    public RegisterTests()
    {
        Authentication.Authenticate(PositionType.Root);
    }

    [Fact]
    public async Task Returns_ok_on_happy_path()
    {
        // Arrange
        var request = CreateRequest();

        // Act
        var response = await Client.PostAsJsonAsync("api/v3.0/employee", request);

        // Assert
        response.Should().Be200Ok();
    }

    [Theory]
    [MemberData(nameof(InvalidRequests))]
    public async Task Returns_bad_request_if_request_is_invalid(RegisterEmployeeRequest request)
    {
        // Arrange
        // Act
        var response = await Client.PostAsJsonAsync("api/v3.0/employee", request);

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
            PositionId = Guid.NewGuid(),
        };
    }
}
