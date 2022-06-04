using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Positions.DataContracts;
using KpiV3.Infrastructure.Adapters;
using KpiV3.WebApi.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace KpiV3.FunctionalTests;

public class TestAuthentication
{
    private HttpClient? _client;

    public void SetClient(HttpClient client)
    {
        _client = client;
    } 

    public JwtOptions Jwt { get; } = new()
    {
        Audience = "TEST_AUDIENCE",
        Issuer = "TEST_ISSUER",
        Secret = "TEST_SECRET_USE_ONLY_FOR_FUNCTIONAL_TESTING_PURPOSES",
        TokenLifetime = TimeSpan.FromHours(1),
    };

    public Employee? Requestor { get; set; }
    public Position? RequestorPosition { get; set; }

    public void Authenticate(PositionType positionType)
    {
        RequestorPosition = new Position
        {
            Id = Guid.NewGuid(),
            Name = "TESTER",
            Type = positionType,
        };

        Requestor = new Employee
        {
            Id = Guid.NewGuid(),
            
            Email = "tester@kpi.com",

            Name = new()
            {
                FirstName = "TESTER_FIRST_NAME",
                LastName = "TESTER_LAST_NAME",
            },

            PositionId = RequestorPosition.Id,
        };

        Authenticate();
    }

    public void Authenticate()
    {
        if (RequestorPosition is null || Requestor is null)
        {
            throw new InvalidOperationException("Requestor and RequestorPosition must be set");
        }

        var jwtFactory = new JwtTokenFactory(
            new DateProvider(),
            Options.Create(Jwt));

        var token = jwtFactory.CreateToken(Requestor, RequestorPosition);

        _client!.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
    }
}
