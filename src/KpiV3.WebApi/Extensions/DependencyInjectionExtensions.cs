using Amazon.S3;
using KpiV3.Domain;
using KpiV3.Domain.Comments.Services;
using KpiV3.Domain.Common.Ports;
using KpiV3.Domain.Employees.Ports;
using KpiV3.Domain.Employees.Services;
using KpiV3.Domain.Files.Ports;
using KpiV3.Domain.Files.Services;
using KpiV3.Domain.Grades.Services;
using KpiV3.Domain.Positions.DataContracts;
using KpiV3.Infrastructure;
using KpiV3.Infrastructure.Common;
using KpiV3.Infrastructure.Employees.EmailService;
using KpiV3.Infrastructure.Employees.PasswordManagement;
using KpiV3.Infrastructure.Files;
using KpiV3.WebApi.Authentication.DataContracts;
using KpiV3.WebApi.Authentication.Services;
using KpiV3.WebApi.HostedServices;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace KpiV3.WebApi.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddKpiV3Services(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        services.AddMediatR(DomainAssembly.Instance, InfrastructureAssembly.Instance);

        services.AddHttpContextAccessor();

        services.AddDbContext<KpiContext>(options =>
        {
            options
                .UseNpgsql(configuration.GetConnectionString("Default"))
                .UseSnakeCaseNamingConvention(CultureInfo.InvariantCulture);
        });

        services
            .AddPrimitiveProviders()
            .AddAuthServices(configuration)
            .AddEmailServices(configuration, environment)
            .AddS3FileStorage(configuration)
            .AddHostedServices(configuration);

        return services.AddDomainServices(configuration);
    }

    private static IServiceCollection AddHostedServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddHostedService<MigrationService>()
            .AddHostedService<DataInitializationService>()
            .Configure<DataInitializationServiceOptions>(configuration.GetSection("DataInitialization"));
    }

    private static IServiceCollection AddAuthServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .Configure<JwtOptions>(configuration.GetSection("Jwt"))
            .AddTransient<IValidateOptions<JwtOptions>, JwtOptionsValidator>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy("RootOnly", policy =>
                policy.RequireClaim("posType", PositionType.Root.ToString()));
        });

        services
            .AddTransient<IJwtTokenService, JwtTokenService>()
            .AddTransient<IJwtTokenFactory, JwtTokenFactory>()
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwt = configuration.GetSection("Jwt").Get<JwtOptions>();
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwt.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwt.Audience,

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = jwt.GetSymmetricSecurityKey(),

                    NameClaimType = "sub",
                    RoleClaimType = "posName",
                };

                options.MapInboundClaims = false;
            });

        return services
            .AddTransient<IEmployeeAccessor, EmployeeAccessor>();
    }

    private static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<EmployeeWelcomeOptions>(configuration.GetSection("Welcome"))
            .AddTransient<IValidateOptions<EmployeeWelcomeOptions>, EmployeeWelcomeOptionsValidator>();

        return services
            .AddTransient<IPasswordHasher, PasswordHasher>()
            .AddTransient<IPasswordGenerator, PasswordGenerator>()
            .AddTransient<EmployeePositionService>()
            .AddTransient<EmployeeWelcomeService>()
            .AddTransient<FileService>()
            .AddTransient<FileOwnershipService>()
            .AddTransient<GradeValidationService>()
            .AddTransient<CommentAuthorityService>()
            .AddTransient<PasswordChangeNotificationService>();
    }

    private static IServiceCollection AddPrimitiveProviders(this IServiceCollection services)
    {
        return services
            .AddTransient<IGuidProvider, GuidProvider>()
            .AddTransient<IDateProvider, DateProvider>();
    }

    private static IServiceCollection AddEmailServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            return services.AddTransient<IEmailService, FakeEmailService>();
        }

        return services
            .Configure<EmailServiceOptions>(configuration.GetSection("EmailService"))
            .AddTransient<IValidateOptions<EmailServiceOptions>, EmailServiceOptionsValidator>()
            .AddTransient<IEmailService, EmailService>();
    }

    private static IServiceCollection AddS3FileStorage(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .Configure<S3FileStorageOptions>(configuration.GetSection("S3"))
            .AddTransient<IValidateOptions<S3FileStorageOptions>, S3FileStorageOptionsValidator>()
            .AddTransient<IFileStorage, S3FileStorage>()
            .AddSingleton(sp =>
            {
                var options = sp.GetRequiredService<IOptions<S3FileStorageOptions>>().Value;

                return new AmazonS3Client(
                    options.AccessKey,
                    options.SecretKey,
                    new AmazonS3Config
                    {
                        ServiceURL = options.Endpoint,
                        ForcePathStyle = true,
                    });
            });

    }
}