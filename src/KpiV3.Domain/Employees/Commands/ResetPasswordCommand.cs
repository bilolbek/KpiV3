using KpiV3.Domain.Employees.Ports;
using KpiV3.Domain.Employees.Services;
using MediatR;

namespace KpiV3.Domain.Employees.Commands;

public record ResetPasswordCommand : IRequest
{
    public Guid EmployeeId { get; init; }
}

public class ResetPasswordCommandHandler : AsyncRequestHandler<ResetPasswordCommand>
{
    private readonly KpiContext _db;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IPasswordGenerator _passwordGenerator;
    private readonly PasswordChangeNotificationService _passwordChangeNotificationService;

    public ResetPasswordCommandHandler(
        KpiContext db,
        IPasswordHasher passwordHasher,
        IPasswordGenerator passwordGenerator,
        PasswordChangeNotificationService passwordChangeNotificationService)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _passwordGenerator = passwordGenerator;
        _passwordChangeNotificationService = passwordChangeNotificationService;
    }

    protected override async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var employee = await _db.Employees
            .FindAsync(new object?[] { request.EmployeeId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        var newPassword = _passwordGenerator.Generate();

        employee.PasswordHash = _passwordHasher.Hash(newPassword);

        await _db.SaveChangesAsync(cancellationToken);

        await _passwordChangeNotificationService.NotifyPasswordChangedAsync(employee, newPassword, cancellationToken);
    }
}
