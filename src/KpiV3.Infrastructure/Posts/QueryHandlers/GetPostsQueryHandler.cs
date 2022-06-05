using KpiV3.Domain.DataContracts.Models;
using KpiV3.Domain.Posts.DataContracts;
using KpiV3.Domain.Posts.Queries;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Posts.Data;
using MediatR;

namespace KpiV3.Infrastructure.Posts.QueryHandlers;

internal class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, Result<Page<PostWithAuthor>, IError>>
{
    private readonly Database _db;

    public GetPostsQueryHandler(Database db)
    {
        _db = db;
    }

    public async Task<Result<Page<PostWithAuthor>, IError>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        const string count = @"SELECT COUNT(*) FROM posts";

        const string select = @"
SELECT
    p.id,
    p.title,
    p.content,
    p.comment_block_id,
    p.written_date,

    e.id as ""author_id"",
    e.first_name as ""author_first_name"",
    e.last_name as ""author_last_name"",
    e.middle_name as ""author_middle_name"",
    e.avatar_id as ""author_avatar_id""
FROM posts p
INNER JOIN employees e on e.id = p.author_id
ORDER BY p.written_date DESC
LIMIT @Limit OFFSET @Offset";

        return await _db
            .QueryFirstAsync<int>(new(count))
            .BindAsync(total => _db.QueryAsync<PostWithAuthorRow>(new(select, new
            {
                request.Pagination.Limit,
                request.Pagination.Offset,
            })).MapAsync(rows => new Page<PostWithAuthorRow>(total, request.Pagination, rows)))
            .MapAsync(rows => rows.Map(row => row.ToModel()));
    }
}
