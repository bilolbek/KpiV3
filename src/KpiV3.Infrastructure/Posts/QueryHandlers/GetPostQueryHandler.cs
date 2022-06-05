using KpiV3.Domain.Posts.DataContracts;
using KpiV3.Domain.Posts.Queries;
using KpiV3.Infrastructure.Data;
using KpiV3.Infrastructure.Posts.Data;
using MediatR;

namespace KpiV3.Infrastructure.Posts.QueryHandlers;

internal class GetPostQueryHandler : IRequestHandler<GetPostQuery, Result<PostWithAuthor, IError>>
{
    private readonly Database _db;

    public GetPostQueryHandler(Database db)
    {
        _db = db;
    }

    public async Task<Result<PostWithAuthor, IError>> Handle(GetPostQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
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
WHERE p.id = @PostId";

        return await _db
            .QueryFirstAsync<PostWithAuthorRow>(new(sql, new { request.PostId }))
            .MapAsync(row => row.ToModel());
    }
}
