using Microsoft.Extensions.Options;

namespace KpiV3.Infrastructure.Employees.Email;

internal class EmailSenderOptionsValidator : IValidateOptions<EmailSenderOptions>
{
    public ValidateOptionsResult Validate(string name, EmailSenderOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.FromAddress))
        {
            return ValidateOptionsResult.Fail($"'{nameof(options.FromAddress)}' is empty");
        }

        if (string.IsNullOrWhiteSpace(options.FromAddressName))
        {
            return ValidateOptionsResult.Fail($"'{nameof(options.FromAddressName)}' is empty");
        }

        if (string.IsNullOrWhiteSpace(options.Password))
        {
            return ValidateOptionsResult.Fail($"'{nameof(options.Password)}' is empty");
        }

        if (string.IsNullOrWhiteSpace(options.SmtpServerAddress))
        {
            return ValidateOptionsResult.Fail($"'{nameof(options.SmtpServerAddress)}' is empty");
        }

        return ValidateOptionsResult.Success;
    }
}
