using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Ports;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;

namespace KpiV3.Infrastructure.Employees.EmailService;

public class EmailService : IEmailService
{
    private readonly EmailServiceOptions _options;

    public EmailService(IOptions<EmailServiceOptions> options)
    {
        _options = options.Value;
    }

    public async Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress(_options.FromAddressName, _options.FromAddress));
        emailMessage.To.Add(new MailboxAddress("", message.Recipient));
        emailMessage.Subject = message.Subject;
        emailMessage.Body = new TextPart(TextFormat.Plain) { Text = message.Body };

        using var client = new SmtpClient();

        await client.ConnectAsync(
            _options.SmtpServerAddress,
            _options.SmtpServerPort,
            _options.SmtpServerSsl);

        await client.AuthenticateAsync(
            _options.FromAddress,
            _options.Password);

        await client.DisconnectAsync(true);
    }
}

public class EmailServiceOptions
{
    public string FromAddress { get; set; } = default!;
    public string FromAddressName { get; set; } = default!;
    public string SmtpServerAddress { get; set; } = default!;
    public int SmtpServerPort { get; set; }
    public bool SmtpServerSsl { get; set; }
    public string Password { get; set; } = default!;
}

public class EmailServiceOptionsValidator : IValidateOptions<EmailServiceOptions>
{
    public ValidateOptionsResult Validate(string name, EmailServiceOptions options)
    {
        if (options is null)
        {
            return ValidateOptionsResult.Fail("Email service options is not configured");
        }

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