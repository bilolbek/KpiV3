using MediatR;

namespace KpiV3.Domain.Employees.Commands;

public record BlockEmployeeCommand : IRequest
{
    public Guid EmployeeId { get; init; }
}

public class BlockEmployeeCommandHandler : AsyncRequestHandler<BlockEmployeeCommand>
{
    private readonly KpiContext _db;

    public BlockEmployeeCommandHandler(KpiContext db)
    {
        _db = db;
    }

    protected override async Task Handle(BlockEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _db.Employees
            .FindAsync(new object?[] { request.EmployeeId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        employee.IsBlocked = true;

        await _db.SaveChangesAsync(cancellationToken);
    }
}
