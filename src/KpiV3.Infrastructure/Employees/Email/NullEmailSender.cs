using KpiV3.Domain.DataContracts.Errors;
using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Ports;
using KpiV3.Rop;
using Microsoft.Extensions.Logging;

namespace KpiV3.Infrastructure.Employees.Email;

public class NullEmailSender : IEmailSender
{
    private readonly ILogger<NullEmailSender> _logger;

    public NullEmailSender(ILogger<NullEmailSender> logger)
    {
        _logger = logger;
    }

    public Task<Result<IError>> SendAsync(EmailMessage message)
    {
        _logger.LogDebug(
            "Email to {Recipient}. Subject: {Subject}. Body: {Body}",
            message.Recipient.ToString(),
            message.Subject,
            message.Body);

        return Task.FromResult(Result<IError>.Ok());
    }
}
