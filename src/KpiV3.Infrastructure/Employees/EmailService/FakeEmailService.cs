using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Ports;
using Microsoft.Extensions.Logging;

namespace KpiV3.Infrastructure.Employees.EmailService;

public class FakeEmailService : IEmailService
{
    private readonly ILogger<FakeEmailService> _logger;

    public FakeEmailService(ILogger<FakeEmailService> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Recipient: {Recipient}. Subject: {Subject}. Body: {Body}",
            message.Recipient,
            message.Subject,
            message.Body);

        return Task.CompletedTask;
    }
}
