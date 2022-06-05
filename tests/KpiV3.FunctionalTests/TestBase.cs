using KpiV3.Domain.Employees.Ports;
using KpiV3.Domain.Periods.Ports;
using KpiV3.Domain.Positions.Ports;
using KpiV3.FunctionalTests.Fakes;
using KpiV3.WebApi.HostedServices.DataInitialization;
using KpiV3.WebApi.HostedServices.FileStorageInitialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace KpiV3.FunctionalTests;

public abstract class TestBase
{
    public TestBase()
    {
        Positions = new();
        Employees = new();
        Periods = new();
        Authentication = new TestAuthentication();
        Client = CreateClient();
        Authentication.SetClient(Client);
    }

    public HttpClient Client { get; }
    public TestAuthentication Authentication { get; }

    public InMemoryPositionRepository Positions { get; }
    public InMemoryEmployeeRepository Employees { get; }
    public InMemoryPeriodRepository Periods { get; }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Transient<IPositionRepository>(_ => Positions));
        services.Replace(ServiceDescriptor.Transient<IEmployeeRepository>(_ => Employees));
        services.Replace(ServiceDescriptor.Transient<IPeriodRepository>(_ => Periods));
    }

    private HttpClient CreateClient()
    {
        return new WebApplicationFactory<Program>().WithWebHostBuilder(host =>
        {
            host.ConfigureServices((context, services) =>
            {
                services.Remove(services.First(d => d.ImplementationType == typeof(DataInitializationService)));
                services.Remove(services.First(d => d.ImplementationType == typeof(FileStorageInitializationService)));

                ConfigureServices(services);
            });

            host.ConfigureAppConfiguration(configuration =>
            {
                var options = new Dictionary<string, string>
                {
                    ["Jwt:Issuer"] = Authentication.Jwt.Issuer,
                    ["Jwt:Audience"] = Authentication.Jwt.Audience,
                    ["Jwt:Secret"] = Authentication.Jwt.Secret,
                    ["Jwt:TokenLifetime"] = Authentication.Jwt.TokenLifetime.ToString(),
                };

                configuration.AddInMemoryCollection(options);
            });
        }).CreateClient();
    }
}
