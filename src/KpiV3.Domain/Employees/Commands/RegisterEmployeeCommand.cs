using KpiV3.Domain.Common;
using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Ports;
using MediatR;

namespace KpiV3.Domain.Employees.Commands;

public record RegisterEmployeeCommand : IRequest<Result<IError>>
{
    public string Email { get; init; } = default!;
    public Name Name { get; init; }
    public Guid PositionId { get; init; }
}

public class RegisterEmployeeCommandHandler : IRequestHandler<RegisterEmployeeCommand, Result<IError>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IPasswordGenerator _passwordGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailSender _emailSender;
    private readonly IGuidProvider _guidProvider;
    private readonly IDateProvider _dateProvider;

    public RegisterEmployeeCommandHandler(
        IEmployeeRepository employeeRepository,
        IPasswordGenerator passwordGenerator,
        IPasswordHasher passwordHasher,
        IEmailSender emailSender,
        IGuidProvider guidProvider,
        IDateProvider dateProvider)
    {
        _employeeRepository = employeeRepository;
        _passwordGenerator = passwordGenerator;
        _passwordHasher = passwordHasher;
        _emailSender = emailSender;
        _guidProvider = guidProvider;
        _dateProvider = dateProvider;
    }

    public async Task<Result<IError>> Handle(RegisterEmployeeCommand request, CancellationToken cancellationToken)
    {
        var password = _passwordGenerator.GeneratePassword();

        var employee = new Employee
        {
            Id = _guidProvider.New(),

            Email = request.Email,
            Name = request.Name,
            PasswordHash = _passwordHasher.Hash(password),

            PositionId = request.PositionId,

            RegistrationDate = _dateProvider.Now(),
        };

        return await _employeeRepository
            .InsertAsync(employee)
            .InsertSuccessAsync(() => new EmailMessage
            {
                Recipient = employee.Email,
                Subject = "KPI Platform Registration",
                Body = $"You have been registered in KPI platform. Your password is: {password}",
            })
            .BindAsync(message => _emailSender.SendAsync(message));
    }
}
