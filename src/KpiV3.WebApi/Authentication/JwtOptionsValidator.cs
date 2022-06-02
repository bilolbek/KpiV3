using Microsoft.Extensions.Options;

namespace KpiV3.WebApi.Authentication;

public class JwtOptionsValidator : IValidateOptions<JwtOptions>
{
    public ValidateOptionsResult Validate(string name, JwtOptions options)
    {
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
