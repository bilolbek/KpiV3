using KpiV3.Domain.DataContracts.Errors;
using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Ports;
using KpiV3.Rop;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace KpiV3.Infrastructure.Employees.Email;

public class EmailSender : IEmailSender
{
    private readonly EmailSenderOptions _options;

    public EmailSender(IOptions<EmailSenderOptions> options)
    {
        _options = options.Value;
    }

    public async Task<Result<IError>> SendAsync(EmailMessage message)
    {
        try
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

            return Result<IError>.Ok();
        }
        catch (Exception exception)
        {
            return Result<IError>.Fail(new InternalError(exception));
        }
    }
}
