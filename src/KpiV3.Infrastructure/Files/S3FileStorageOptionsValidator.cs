using Microsoft.Extensions.Options;

namespace KpiV3.Infrastructure.Files;

public class S3FileStorageOptionsValidator : IValidateOptions<S3FileStorageOptions>
{
    public ValidateOptionsResult Validate(string name, S3FileStorageOptions options)
    {
        if (options is null)
        {
            return ValidateOptionsResult.Fail("S3 file storage options is not configured");
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