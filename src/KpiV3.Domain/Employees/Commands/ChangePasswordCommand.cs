using KpiV3.Domain.Employees.DataContracts;
using KpiV3.Domain.Employees.Ports;
using MediatR;

namespace KpiV3.Domain.Employees.Commands;

public record ChangePasswordCommand : IRequest<Result<IError>>
{
    public Guid EmployeeId { get; set; }
    public string OldPassword { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}


public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result<IError>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IPasswordHasher _passwordHasher;

    public ChangePasswordCommandHandler(
        IEmployeeRepository employeeRepository,
        IPasswordHasher passwordHasher)
    {
        _employeeRepository = employeeRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<IError>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        return await _employeeRepository
            .FindByIdAsync(request.EmployeeId)
            .BindAsync(employee =>
            {
                if (!_passwordHasher.Verify(request.OldPassword, employee.PasswordHash))
                {
                    return Result<Employee, IError>.Fail(new ForbidenAction("Wrong password"));
                }

                return Result<Employee, IError>.Ok(employee with { PasswordHash = _passwordHasher.Hash(request.NewPassword) });
            })
            .BindAsync(employee => _employeeRepository.UpdateAsync(employee));
    }
}
