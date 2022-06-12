namespace KpiV3.Domain.Files.Services;

public class FileOwnershipService
{
    private readonly KpiContext _db;

    public FileOwnershipService(KpiContext db)
    {
        _db = db;
    }

    public async Task EnsureEmployeeOwnsFilesAsync(
        Guid employeeId,
        ICollection<Guid> fileIds,
        CancellationToken cancellationToken = default)
    {
        var files = await _db.Files
            .Where(f => fileIds.Contains(f.Id))
            .ToListAsync(cancellationToken);

        if (!files.All(f => f.OwnerId == employeeId))
        {
            throw new ForbiddenActionException("One or more of the requeset files does not belong to you");
        }
    }
}
