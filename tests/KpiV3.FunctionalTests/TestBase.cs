using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Infrastructure.Adapters;
using KpiV3.WebApi.Authentication;
using KpiV3.WebApi.HostedServices.DataInitialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
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

        var jwtFactory = new JwtTokenFactory(
            new DateProvider(),
            Options.Create(Jwt));

        var token = jwtFactory.CreateToken(Requestor, RequestorPosition);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

        return client;
    }

    protected HttpClient CreateClient(Action<IWebHostEnvironment, IServiceCollection> configure)
    {
        var application = new WebApplicationFactory<Program>().WithWebHostBuilder(host =>
        {
            host.ConfigureServices((context, services) =>
            {
                services.Remove(services.First(d => d.ImplementationType == typeof(DataInitializationService)));


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
