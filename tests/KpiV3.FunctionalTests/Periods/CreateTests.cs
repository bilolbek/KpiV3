using FluentAssertions;
using KpiV3.Domain.Positions.DataContracts;
using KpiV3.WebApi.DataContracts.Periods;
using System.Net.Http.Json;

namespace KpiV3.FunctionalTests.Periods;

public class CreateTests : TestBase
{
    public CreateTests()
    {
        Authentication.Authenticate(PositionType.Root);
    }

    [Fact]
    public async Task Returns_ok_and_created_period_on_happy_path()
    {
        // Arrange
        var request = CreateRequest();

        // Act
        var response = await Client.PostAsJsonAsync("api/v3.0/period", request);

        // Assert
        response.Should().Be200Ok().And.Satisfy<PeriodDto>(period =>
        {
            period.Name.Should().Be(request.Name);
            period.From.Should().Be(request.From);
            period.To.Should().Be(request.To);
        });
    }

    [Fact]
    public async Task Creates_period_on_happy_path()
    {
        // Arrange
        var request = CreateRequest();

        // Act
        var response = await Client.PostAsJsonAsync("api/v3.0/period", request);

        // Assert
        Periods.Items.Values.Should().Contain(p =>
            p.Name == request.Name &&
            p.From == request.From &&
            p.To == request.To);
    }

    [Theory]
    [MemberData(nameof(InvalidRequests))]
    public async Task Returns_bad_request_on_invalid_request(CreatePeriodRequest request)
    {
        // Arrange
        // Act
        var response = await Client.PostAsJsonAsync("api/v3.0/period", request);

        // Assert
        response.Should().Be400BadRequest();
    }

    private static IEnumerable<object[]> InvalidRequests()
    {
        var request = CreateRequest();

        yield return new object[] { request with { Name = "" } };
        yield return new object[] { request with { From = request.To.AddTicks(1) } };
    }

    private static CreatePeriodRequest CreateRequest()
    {
        return new CreatePeriodRequest
        {
            Name = "2021 year",
            From = new DateTime(2021, 1, 1),
            To = new DateTime(2021, 12, 31)
        };
    }
}
