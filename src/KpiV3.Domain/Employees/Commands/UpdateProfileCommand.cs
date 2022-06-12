using KpiV3.Domain.Common.DataContracts;
using MediatR;

namespace KpiV3.Domain.Employees.Commands;

public record UpdateProfileCommand : IRequest
{
    public Guid EmployeeId { get; init; }
    public Name Name { get; init; } = default!;
    public Guid? AvatarId { get; init; }
}

public class UpdateProfileCommandHandler : AsyncRequestHandler<UpdateProfileCommand>
{
    private readonly KpiContext _db;

    public UpdateProfileCommandHandler(KpiContext db)
    {
        _db = db;
    }

    protected override async Task Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var employee = await _db.Employees
            .FindAsync(new object?[] { request.EmployeeId }, cancellationToken: cancellationToken)
            .EnsureFoundAsync();

        employee.Name = request.Name;
        employee.AvatarId = request.AvatarId;

        await _db.SaveChangesAsync(cancellationToken);
    }
}
