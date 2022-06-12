using KpiV3.Domain.Positions.DataContracts;

namespace KpiV3.Domain.Employees.Services;

public class EmployeePositionService
{
    private readonly KpiContext _db;

    public EmployeePositionService(KpiContext db)
    {
        _db = db;
    }

    public async Task<bool> EmployeeHasRootPositionAsync(
        Guid employeeId,
        CancellationToken cancellationToken)
    {
        return await _db.Employees
            .Where(e => e.Id == employeeId)
            .Select(e => e.Position.Type == PositionType.Root)
            .AnyAsync(cancellationToken);
    }
}
