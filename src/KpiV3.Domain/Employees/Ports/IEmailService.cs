using KpiV3.Domain.Employees.DataContracts;

namespace KpiV3.Domain.Employees.Ports;

public interface IEmailService
{
    Task SendEmailAsync(
        EmailMessage message, 
        CancellationToken cancellationToken = default);
}
