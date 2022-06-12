using MediatR;

namespace KpiV3.Domain.Employees.Commands;

public record UnblockEmployeeCommand : IRequest
{
    public Guid EmployeeId { get; init; }
}

public class UnblockEmployeeCommandHandler : AsyncRequestHandler<UnblockEmployeeCommand>
{
    private readonly KpiContext _db;

    public UnblockEmployeeCommandHandler(KpiContext db)
    {
        _db = db;
    }

    protected override async Task Handle(UnblockEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _db.Employees
           .FindAsync(new object?[] { request.EmployeeId }, cancellationToken: cancellationToken)
           .EnsureFoundAsync();

        employee.IsBlocked = false;

        await _db.SaveChangesAsync(cancellationToken);
    }
}
