using KpiV3.Domain.Employees.DataContracts;

namespace KpiV3.Domain.Employees.Ports;

public interface IEmailSender
{
    Task<Result<IError>> SendAsync(EmailMessage message);
}
