using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace KpiV3.WebApi.Authentication.DataContracts;

public record JwtOptions
{
    public string Audience { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public string Secret { get; set; } = default!;
    public TimeSpan TokenLifetime { get; set; }

    public SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
    }
}

public class JwtOptionsValidator : IValidateOptions<JwtOptions>
{
    public ValidateOptionsResult Validate(string name, JwtOptions options)
    {
        if (options is null)
        {
            return ValidateOptionsResult.Fail("Jwt options is not configured");
        }

        if (string.IsNullOrWhiteSpace(options.Secret))
        {
            return ValidateOptionsResult.Fail($"'{nameof(JwtOptions.Secret)}' cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(options.Issuer))
        {
            return ValidateOptionsResult.Fail($"'{nameof(JwtOptions.Issuer)}' cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(options.Audience))
        {
            return ValidateOptionsResult.Fail($"'{nameof(JwtOptions.Audience)}' cannot be empty.");
        }

        if (options.TokenLifetime <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail($"'{nameof(JwtOptions.TokenLifetime)}' must be greater than zero.");
        }

        return ValidateOptionsResult.Success;
    }
}
