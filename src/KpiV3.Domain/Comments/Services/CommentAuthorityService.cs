using KpiV3.Domain.Positions.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KpiV3.Domain.Comments.Services;

public class CommentAuthorityService
{
    private readonly KpiContext _db;

    public CommentAuthorityService(KpiContext db)
    {
        _db = db;
    }

    public async Task EnsureEmployeeCanModifyCommentAsync(
        Guid employeeId,
        Guid commentId,
        CancellationToken cancellationToken = default)
    {
        var employeeHasRootPrivilege = await _db.Employees
            .AnyAsync(e => e.Id == employeeId && e.Position.Type == PositionType.Root, cancellationToken);

        if (employeeHasRootPrivilege)
        {
            return;
        }

        var employeeIsCommentsAuthor = await _db.Comments
            .AnyAsync(c => c.Id == commentId && c.AuthorId == employeeId, cancellationToken);

        if (!employeeIsCommentsAuthor)
        {
            throw new ForbiddenActionException("You can't modify this comment");
        }
    }
}
