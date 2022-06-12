using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KpiV3.Domain.Employees.Services;

public class PasswordChangeNotificationService
{
    private readonly IEmailService _emailService;

    public PasswordChangeNotificationService(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task NotifyPasswordChangedAsync(
        Employee employee,
        string newPassword,
        CancellationToken cancellationToken)
    {
        var message = new EmailMessage
        {
            Recipient = employee.Email,
            Subject = "Password has been reset",
            Body = $"Hello, {employee.Name.FirstName}! Your password has been reset to: {newPassword}",
        };

        await _emailService.SendEmailAsync(message, cancellationToken);
    }
}
