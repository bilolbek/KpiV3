using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Ports;
using MediatR;

namespace KpiV3.Domain.Employees.Commands;

public record ResetPasswordCommand : IRequest<Result<IError>>
{
    public Guid EmployeeId { get; set; }
}

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<IError>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IPasswordGenerator _passwordGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailSender _emailSender;

    public ResetPasswordCommandHandler(
        IEmployeeRepository employeeRepository,
        IPasswordGenerator passwordGenerator,
        IPasswordHasher passwordHasher,
        IEmailSender emailSender)
    {
        _employeeRepository = employeeRepository;
        _passwordGenerator = passwordGenerator;
        _passwordHasher = passwordHasher;
        _emailSender = emailSender;
    }

    public async Task<Result<IError>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var password = _passwordGenerator.GeneratePassword();

        return await _employeeRepository
            .FindByIdAsync(request.EmployeeId)
            .MapAsync(employee => employee with { PasswordHash = _passwordHasher.Hash(password) })
            .BindAsync(employee => _employeeRepository
                .UpdateAsync(employee)
                .BindAsync(() => _emailSender.SendAsync(new EmailMessage
                {
                    Subject = "Password reset",
                    Body = $"Your password has been reset. Your new password: {password}",
                    Recipient = employee.Email,
                })));
    }
}
