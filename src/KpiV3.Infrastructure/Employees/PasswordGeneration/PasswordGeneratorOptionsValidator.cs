using Microsoft.Extensions.Options;

namespace KpiV3.Infrastructure.Employees.PasswordGeneration;

internal class PasswordGeneratorOptionsValidator : IValidateOptions<PasswordGeneratorOptions>
{
    public ValidateOptionsResult Validate(string name, PasswordGeneratorOptions options)
    {
        if (options is null)
        {
            return ValidateOptionsResult.Fail("Configuration object is null");
        }

        if (options.Length <= 0)
        {
            return ValidateOptionsResult.Fail("Length must be greater than '0'");
        }

        return ValidateOptionsResult.Success;
    }
}
