using KpiV3.Domain.Employees.Ports;
using KpiV3.Infrastructure.Employees.Crypto;
using KpiV3.Infrastructure.Employees.Email;
using KpiV3.Infrastructure.Employees.PasswordGeneration;
using KpiV3.Infrastructure.Employees.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace KpiV3.Infrastructure.Employees.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddEmployeeAdapters(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        return services
            .AddPasswordGenerator(configuration)
            .AddPasswordHasher()
            .AddEmailSender(configuration, environment)
            .AddRepositories();
    }

    private static IServiceCollection AddPasswordHasher(this IServiceCollection services)
    {
        return services
            .AddTransient<IPasswordHasher, BcryptPasswordHasher>();
    }

    private static IServiceCollection AddPasswordGenerator(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddTransient<IPasswordGenerator, PasswordGenerator>()
            .AddTransient<IValidateOptions<PasswordGeneratorOptions>, PasswordGeneratorOptionsValidator>()
            .Configure<PasswordGeneratorOptions>(configuration.GetSection("PasswordGenerator"));
    }

    private static IServiceCollection AddEmailSender(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            services.AddTransient<IEmailSender, NullEmailSender>();
        }
        else
        {
            services
                .AddTransient<IEmailSender, EmailSender>()
                .AddTransient<IValidateOptions<EmailSenderOptions>, EmailSenderOptionsValidator>()
                .Configure<EmailSenderOptions>(configuration.GetSection("EmailSender"));
        }

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddTransient<IEmployeeRepository, EmployeeRepository>()
            .AddTransient<IPositionRepository, PositionRepository>();
    }
}
