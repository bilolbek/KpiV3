using KpiV3.Domain.Employees.Ports;
using MediatR;

namespace KpiV3.Domain.Employees.Commands;

public record ChangePasswordCommand : IRequest
{
    public Guid EmployeeId { get; init; }
    public string OldPassword { get; init; } = default!;
    public string NewPassword { get; init; } = default!;
}

public class ChangePasswordCommandHandler : AsyncRequestHandler<ChangePasswordCommand>
{
    private readonly KpiContext _db;
    private readonly IPasswordHasher _passwordHasher;

    public ChangePasswordCommandHandler(
        KpiContext db,
        IPasswordHasher passwordHasher)
    {
        _db = db;
        _passwordHasher = passwordHasher;
    }

    protected override async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var employee = await _db.Employees
            .FindAsync(new object?[] { request.EmployeeId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        if (!_passwordHasher.Verify(request.OldPassword, employee.PasswordHash))
        {
            throw new InvalidInputException("Old password is wrong");
        }

        employee.PasswordHash = _passwordHasher.Hash(request.NewPassword);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
