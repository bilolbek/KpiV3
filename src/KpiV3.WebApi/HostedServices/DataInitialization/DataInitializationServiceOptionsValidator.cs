using Microsoft.Extensions.Options;

namespace KpiV3.WebApi.HostedServices.DataInitialization;

public class DataInitializationServiceOptionsValidator : IValidateOptions<DataInitializationServiceOptions>
{
    public ValidateOptionsResult Validate(string name, DataInitializationServiceOptions options)
    {
        if (options is null)
        {
            return ValidateOptionsResult.Fail("Configuration object is null");
        }

        if (options.Employees is null)
        {
            return ValidateOptionsResult.Fail($"'{nameof(options.Employees)}' was null");
        }

        if (options.Positions is null)
        {
            return ValidateOptionsResult.Fail($"'{nameof(options.Positions)}' was null");
        }

        if (options.RetryCount < 0)
        {
            return ValidateOptionsResult.Fail($"'{nameof(options.RetryCount)}' was less than 0");
        }

        if (options.RetryWait < TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail($"'{nameof(options.RetryWait)}' was less than 0");
        }

        return ValidateOptionsResult.Success;
    }
}
