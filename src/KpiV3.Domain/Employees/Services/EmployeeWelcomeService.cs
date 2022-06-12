using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Ports;
using Microsoft.Extensions.Options;

namespace KpiV3.Domain.Employees.Services;

public class EmployeeWelcomeService
{
    private readonly IEmailService _emailService;
    private readonly EmployeeWelcomeOptions _options;

    public EmployeeWelcomeService(
        IEmailService emailService,
        IOptions<EmployeeWelcomeOptions> options)
    {
        _emailService = emailService;
        _options = options.Value;
    }

    public async Task SendWelcomeMessageAsync(
        Employee employee, 
        string password,
        CancellationToken cancellationToken = default)
    {
        var message = new EmailMessage
        {
            Subject = _options.Subject,
            Body = string.Format(_options.BodyTemplate, employee.Name.FirstName, password),
        };

        await _emailService.SendEmailAsync(message, cancellationToken);
    }
}

public class EmployeeWelcomeOptions
{
    public string Subject { get; init; } = default!;
    public string BodyTemplate { get; init; } = default!;
}

public class EmployeeWelcomeOptionsValidator : IValidateOptions<EmployeeWelcomeOptions>
{
    public ValidateOptionsResult Validate(string name, EmployeeWelcomeOptions options)
    {
        if (options is null)
        {
            return ValidateOptionsResult.Fail("Employee welcome options is not configured");
        }

        if (string.IsNullOrWhiteSpace(options.Subject))
        {
            return ValidateOptionsResult.Fail($"'{nameof(options.Subject)}' is empty");
        }

        if (string.IsNullOrWhiteSpace(options.BodyTemplate))
        {
            return ValidateOptionsResult.Fail($"'{nameof(options.BodyTemplate)}' is empty");
        }

        return ValidateOptionsResult.Success;
    }
}
