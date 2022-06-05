using KpiV3.Domain.Comments.DataContracts;
using KpiV3.Domain.Comments.Queries;
using KpiV3.Domain.DataContracts.Models;
using KpiV3.Infrastructure.Comments.Data;
using KpiV3.Infrastructure.Data;
using MediatR;

namespace KpiV3.Infrastructure.Comments.QueryHandlers;

internal class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery, Result<Page<CommentWithAuthor>, IError>>
{
    private readonly Database _db;

    public GetCommentsQueryHandler(Database db)
    {
        _db = db;
    }

    public async Task<Result<Page<CommentWithAuthor>, IError>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        const string count = @"
SELECT COUNT(*) FROM comments
WHERE block_id = @BlockId";

        const string select = @"
SELECT 
    c.id,
    c.content,
    c.written_date,
    c.block_id,
    e.id as ""author_id"",
    e.first_name as ""author_first_name"",
    e.last_name as ""author_last_name"",
    e.middle_name as ""author_middle_name"",
    e.avatar_id as ""author_avatar_id""
FROM comments c
INNER JOIN employees e on c.author_id = e.id
WHERE block_id = @BlockId
LIMIT @Limit OFFSET @Offset";

        return await _db
            .QueryFirstAsync<int>(new(count, new { request.BlockId }))
            .BindAsync(total => _db.QueryAsync<CommentWithAuthorRow>(new(select, new
            {
                request.Pagination.Limit,
                request.Pagination.Offset,
                request.BlockId,
            })).MapAsync(rows => new Page<CommentWithAuthorRow>(total, request.Pagination, rows)))
            .MapAsync(rows => rows.Map(row => row.ToModel()));
    }
}
