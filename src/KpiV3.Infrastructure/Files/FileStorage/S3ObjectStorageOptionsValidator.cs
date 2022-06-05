using Microsoft.Extensions.Options;

namespace KpiV3.Infrastructure.Files.FileStorage;

public class S3ObjectStorageOptionsValidator : IValidateOptions<S3ObjectStorageOptions>
{
    public ValidateOptionsResult Validate(string name, S3ObjectStorageOptions options)
    {
        if (options is null)
        {
            return ValidateOptionsResult.Fail("Configuration object is null");
        }
        
        if (string.IsNullOrWhiteSpace(options.Endpoint))
        {
            return ValidateOptionsResult.Fail($"'{nameof(options.Endpoint)}' is empty");
        }

        if (string.IsNullOrWhiteSpace(options.Bucket))
        {
            return ValidateOptionsResult.Fail($"'{nameof(options.Bucket)}' is empty");
        }

        if (string.IsNullOrWhiteSpace(options.AccessKey))
        {
            return ValidateOptionsResult.Fail($"'{nameof(options.AccessKey)}' is empty");
        }

        if (string.IsNullOrWhiteSpace(options.SecretKey))
        {
            return ValidateOptionsResult.Fail($"'{nameof(options.SecretKey)}' is empty");
        }

        return ValidateOptionsResult.Success;
    }
}
