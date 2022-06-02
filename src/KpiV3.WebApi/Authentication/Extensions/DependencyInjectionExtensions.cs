using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace KpiV3.WebApi.Authentication.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        services
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
            .Configure<JwtOptions>(configuration.GetSection("Jwt"))
            .AddTransient<IValidateOptions<JwtOptions>, JwtOptionsValidator>()
            .AddTransient<IEmployeeAccessor, HttpContextEmployeeAccessor>()
            .AddTransient<IJwtTokenFactory, JwtTokenFactory>();
    }
}
