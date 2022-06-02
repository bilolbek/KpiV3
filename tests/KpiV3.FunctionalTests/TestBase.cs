using KpiV3.Domain.Employees.DataContracts;
using KpiV3.WebApi.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;

namespace KpiV3.FunctionalTests;

public abstract class TestBase
{
    public Employee? Requestor { get; set; }
    public Position? RequestorPosition { get; set; }

    protected DateTimeOffset JwtIssueTime { get; set; } = DateTimeOffset.Now;

    protected JwtOptions Jwt { get; } = new()
    {
        Audience = "TEST_AUDIENCE",
        Issuer = "TEST_ISSUER",
        Secret = "TEST_SECRET_USE_ONLY_FOR_FUNCTIONAL_TESTING_PURPOSES",
        TokenLifetime = TimeSpan.FromHours(1),
    };

    protected HttpClient Authorize(HttpClient client)
    {
        if (RequestorPosition is null || Requestor is null)
        {
            throw new InvalidOperationException("Must set Requestor and his position to authorize client");
        }

        var token = IssueToken(Requestor, RequestorPosition);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return client;
    }

    private string IssueToken(Employee employee, Position position)
    {
        var handler = new JsonWebTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = Jwt.Issuer,
            Audience = Jwt.Audience,
            IssuedAt = JwtIssueTime.UtcDateTime,
            Expires = JwtIssueTime.Add(Jwt.TokenLifetime).UtcDateTime,
            Claims = new Dictionary<string, object>
            {
                { "sub", employee.Id.ToString() },
                { "posId", position.Id.ToString() },
                { "posName", position.Name },
                { "firstName", employee.Name.FirstName },
                { "lastName", employee.Name.LastName },
            },
            SigningCredentials = new SigningCredentials(
                Jwt.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256),
        };

        if (employee.Name.MiddleName is not null)
        {
            tokenDescriptor.Claims["middleName"] = employee.Name.MiddleName;
        }

        return handler.CreateToken(tokenDescriptor);
    }

    protected HttpClient CreateClient(Action<IWebHostEnvironment, IServiceCollection> configure)
    {
        var application = new WebApplicationFactory<Program>().WithWebHostBuilder(host =>
        {
            host.ConfigureServices((context, services) =>
            {
                configure(context.HostingEnvironment, services);
            });

            host.ConfigureAppConfiguration(configuration =>
            {
                var options = new Dictionary<string, string>
                {
                    ["Jwt:Issuer"] = Jwt.Issuer,
                    ["Jwt:Audience"] = Jwt.Audience,
                    ["Jwt:Secret"] = Jwt.Secret,
                    ["Jwt:TokenLifetime"] = Jwt.TokenLifetime.ToString(),
                };

                configuration.AddInMemoryCollection(options);
            });
        });

        return application.CreateClient();
    }
}
