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
        var ownedCount = await _db.Files
            .CountAsync(f =>
                f.OwnerId == employeeId &&
                fileIds.Contains(f.Id), cancellationToken);

        if (fileIds.Count != ownedCount)
        {
            throw new ForbiddenActionException("One or more of the requeset files does not belong to you");
        }
    }
}
